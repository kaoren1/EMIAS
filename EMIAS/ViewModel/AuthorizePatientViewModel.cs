using EMIAS.View;
using EMIAS.ViewModel.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMIAS.ViewModel
{
    public partial class AuthorizePatientViewModel : INotifyPropertyChanged
    {
        public BindableCommand PatientAuthorize { get; set; }
        public BindableCommand SelectADWindow { get; set; }


        HttpClient httpClient;

        private string _oms;

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

        private string _apiResponse;
        public string ApiResponse
        {
            get { return _apiResponse; }
            set
            {
                _apiResponse = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AuthorizePatientViewModel()
        {
            SelectADWindow = new BindableCommand(_ => SelectWindow());
            PatientAuthorize = new BindableCommand(_ => GetPatient());
        }

        private async Task GetPatient()
        {
            if (string.IsNullOrEmpty(OMS))
            {
                return;
            }

            try
            {
                var apiUrl = $"http://localhost:7084/getoms/{OMS}";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    ApiResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Успех");
                }
                else
                {
                    MessageBox.Show("Ошибка: Запрос не выполнен.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void SelectWindow()
        {
            AuthorizePatientWindow a = new AuthorizePatientWindow();
            a.Show();
        }
    }
}
