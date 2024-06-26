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

        HttpClient httpClient;
        //Оригинальный
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
            PatientAuthorize = new BindableCommand(async _ => await GetPatient());
            httpClient = new HttpClient();
        }

        private async Task GetPatient()
        {
            if (string.IsNullOrEmpty(OMS))
            {
                return;
            }

            try
            {
                var apiUrl = $"http://localhost:5181/getoms/{OMS}";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    MainPatientWindow m = new MainPatientWindow();
                    m.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex);
            }
        }
    }
}