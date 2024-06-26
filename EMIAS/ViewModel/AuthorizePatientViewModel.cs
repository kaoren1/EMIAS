using EMIAS.Model;
using EMIAS.View;
using EMIAS.ViewModel.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EMIAS.ViewModel
{
    public partial class AuthorizePatientViewModel : INotifyPropertyChanged
    {
        public BindableCommand PatientAuthorize { get; set; }

        HttpClient httpClient;

        private string _oms;
        private Patient _selectedPatient;

        public event PropertyChangedEventHandler PropertyChanged;

        public string OMS
        {
            get { return _oms; }
            set
            {
                _oms = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Patient> Patients { get; set; }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged();
            }
        }

        public AuthorizePatientViewModel()
        {
            httpClient = new HttpClient();
            Patients = new ObservableCollection<Patient>();
            PatientAuthorize = new BindableCommand(async _ => await AuthorizePatient());

           
        }

        private async Task AuthorizePatient()
        {
            if (string.IsNullOrEmpty(OMS))
            {
                MessageBox.Show("Пожалуйста, введите ОМС и пароль.");
                return;
            }

            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/Patients/{OMS}");

                if (response.IsSuccessStatusCode)
                {
                    var patientJson = await response.Content.ReadAsStringAsync();
                    var patient = JsonConvert.DeserializeObject<Patient>(patientJson);

                    if (patient != null && patient.Oms == long.Parse(OMS))
                    {

                        if (!Patients.Contains(patient))
                        {
                            Patients.Add(patient);
                        }
                        SelectedPatient = patient;

                        var mainWindow = new MainPatientWindow();
                        mainWindow.DataContext = this; 
                        mainWindow.Show();

                
                        var frame = mainWindow.FindName("PageFrame") as Frame;
                        frame.Navigate(new MakeAppointmentPage());

                        Application.Current.Windows[0]?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка авторизации: неверный ОМС.");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации: неверный ОМС.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        //private async void LoadAllPatients()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync("http://localhost:5181/api/Patients");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var patientsJson = await response.Content.ReadAsStringAsync();
        //            var patients = JsonConvert.DeserializeObject<ObservableCollection<Patient>>(patientsJson);

        //            if (patients != null)
        //            {
        //                Patients.Clear();
        //                foreach (var patient in patients)
        //                {
        //                    Patients.Add(patient);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Ошибка загрузки списка пациентов.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка: " + ex.Message);
        //    }
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
