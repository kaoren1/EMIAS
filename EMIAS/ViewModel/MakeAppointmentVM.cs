using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using EMIAS.Model;
using Newtonsoft.Json;

namespace EMIAS.ViewModel
{
    public class MakeAppointmentVM : INotifyPropertyChanged
    {
        private HttpClient httpClient;
        private string _oms;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Speciality1> _specialities;
        public ObservableCollection<Speciality1> Specialities
        {
            get { return _specialities; }
            set
            {
                _specialities = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AppointmentMonth> _appointmentMonths;
        public ObservableCollection<AppointmentMonth> AppointmentMonths { get; set; }
        public string OMS
        {
            get { return _oms; }
            set
            {
                _oms = value;
                OnPropertyChanged();
                LoadAppointmentsForMonths();
            }
        }
        public MakeAppointmentVM()
        {
            httpClient = new HttpClient();
            Specialities = new ObservableCollection<Speciality1>();
            LoadSpecialities();
            LoadAppointmentMonths();
        }

        private void LoadAppointmentMonths()
        {
            AppointmentMonths = new ObservableCollection<AppointmentMonth>();
            var now = DateTime.Now;

            for (int i = 0; i < 12; i++)
            {
                var appointmentMonth = new AppointmentMonth
                {
                    MonthYear = now.AddMonths(i).ToString("MMMM yyyy")
                };
                AppointmentMonths.Add(appointmentMonth);
            }
        }

        private async void LoadAppointmentsForMonths()
        {

            if (string.IsNullOrEmpty(OMS))
                return;

            foreach (var month in AppointmentMonths)
            {
                await month.LoadAppointments(long.Parse(OMS));
            }

        }



        private async void LoadSpecialities()
        {
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5181/getDoctorSpecialities");

                if (response.IsSuccessStatusCode)
                {
                    var specialitiesJson = await response.Content.ReadAsStringAsync();
                    var specialities = JsonConvert.DeserializeObject<ObservableCollection<Speciality1>>(specialitiesJson);

                    if (specialities != null)
                    {
                        Specialities.Clear();
                        foreach (var speciality in specialities)
                        {
                            speciality.ImagePath = GetImagePath(speciality.Name);
                            Specialities.Add(speciality);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки списка специальностей.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

       

        private string GetImagePath(string specialityName)
        {
            switch (specialityName)
            {
                case "Дежурный врач ОРВИ":
                    return "pack://application:,,,/Images/orvi.png";
                case "Стоматолог-терапевт":
                    return "pack://application:,,,/Images/tooth.png";
                case "Офтальмолог":
                    return "pack://application:,,,/Images/glasses.png";
                case "Аллерголог":
                    return "pack://application:,,,/Images/vaccine.png";
                case "Стоматолог-хирург":
                    return "pack://application:,,,/Images/tooth.png";
                case "Участковый врач":
                    return "pack://application:,,,/Images/orvi.png";
                case "Оториноларинголог":
                    return "pack://application:,,,/Images/hearing.png";
                case "Хирург":
                    return "pack://application:,,,/Images/hirurg.png";
                case "Эндокринолог":
                    return "pack://application:,,,/Images/heart.png";
                default:
                    return "pack://application:,,,/Images/orvi.png";
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AppointmentMonth
    {
        public string MonthYear { get; set; }
        public ObservableCollection<Appointment1> Appointments { get; set; }

        public AppointmentMonth()
        {
            Appointments = new ObservableCollection<Appointment1>();
        }

        public async Task LoadAppointments(long oms)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"http://localhost:5181//getActiveAppointments/{oms}");
                if (response.IsSuccessStatusCode)
                {
                    var appointmentsJson = await response.Content.ReadAsStringAsync();
                    var appointments = JsonConvert.DeserializeObject<List<Appointment1>>(appointmentsJson);

                    foreach (var appointment in appointments)
                    {
                        if (appointment.AppointmentDate.ToString("MMMM yyyy") == this.MonthYear)
                        {
                            Appointments.Add(appointment);
                        }
                    }
                }
            }
        }
    }

}
