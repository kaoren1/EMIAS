using EMIAS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMIAS.ViewModel
{
    public class MakeAppointment2VM
    {
        private HttpClient httpClient;
        private string _oms;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Speciality> _specialities;
        public ObservableCollection<Speciality> Specialities
        {
            get { return _specialities; }
            set
            {
                _specialities = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Direction> _directions;
        public ObservableCollection<Direction> Directions
        {
            get { return _directions; }
            set
            {
                _directions = value;
                OnPropertyChanged();
            }
        }

         public string OMS
        {
            get { return _oms; }
            set
            {
                _oms = value;
                OnPropertyChanged();
                LoadDirections();
            }
        }
        public MakeAppointment2VM()
        {
            httpClient = new HttpClient();
            Specialities = new ObservableCollection<Speciality>();
            LoadSpecialities();
            LoadDirections();
        }

        private async void LoadDirections()
        {
            if (string.IsNullOrEmpty(OMS))
                return;

            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/Directions?oms={OMS}");

                if (response.IsSuccessStatusCode)
                {
                    var directionsJson = await response.Content.ReadAsStringAsync();
                    var directions = JsonConvert.DeserializeObject<ObservableCollection<Direction>>(directionsJson);

                    if (directions != null)
                    {
                        Directions.Clear();
                        foreach (var direction in directions)
                        {
                            Directions.Add(direction);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки списка направлений.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void LoadDirectionSpecialities()
        {
            foreach (var direction in Directions)
            {
                direction.Speciality = Specialities.FirstOrDefault(s => s.IdSpeciality == direction.SpecialityId);
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
                    var specialities = JsonConvert.DeserializeObject<ObservableCollection<Speciality>>(specialitiesJson);

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
}
