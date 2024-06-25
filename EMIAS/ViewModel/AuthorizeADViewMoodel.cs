using EMIAS.Model;
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
    public partial class AuthorizeADViewModel : INotifyPropertyChanged
    {
        private HttpClient httpClient;

        private string _loginId;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string LoginId
        {
            get { return _loginId; }
            set
            {
                _loginId = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
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

        public AuthorizeADViewModel()
        {
            httpClient = new HttpClient();
            LoginCommand = new BindableCommand(_ => Login());
            SelectCommand = new BindableCommand(_ => SelectPatientWindow());
        }

        public BindableCommand LoginCommand { get; set; }
        public BindableCommand SelectCommand { get; set; }


        private async Task Login()
        {
            if (string.IsNullOrEmpty(LoginId) || string.IsNullOrEmpty(Password))
            {
                return;
            }

            try
            {
                // Запрос к Doctor API
                var doctorApiUrl = "http://localhost:7084/getloginDoctor";
                var doctorLoginModel = new LoginModelDoctor { Id = LoginId, Password = Password };
                var doctorJson = JsonConvert.SerializeObject(doctorLoginModel);
                var doctorContent = new StringContent(doctorJson, System.Text.Encoding.UTF8, "application/json");

                var doctorTask = httpClient.PostAsync(doctorApiUrl, doctorContent);

                // Запрос к Admin API
                var adminApiUrl = "http://localhost:7084/getloginAdmin";
                var adminLoginModel = new LoginModelAdmin { IdAdmin = LoginId, EnterPassword = Password };
                var adminJson = JsonConvert.SerializeObject(adminLoginModel);
                var adminContent = new StringContent(adminJson, System.Text.Encoding.UTF8, "application/json");

                var adminTask = httpClient.PostAsync(adminApiUrl, adminContent);

                // Ожидание завершения обоих запросов
                await Task.WhenAll(doctorTask, adminTask);

                if (doctorTask.Result.IsSuccessStatusCode)
                {
                    // Действия для успешной авторизации Doctor
                    /*DoctorMainWindow doctorMainWindow = new DoctorMainWindow(сюда айди доктора);
                    doctorMainWindow.Show();*/
                    MessageBox.Show(doctorLoginModel.Id);
                }
                else if (adminTask.Result.IsSuccessStatusCode)
                {
                    // Действия для успешной авторизации Admin
                }
                else
                {
                    // Действия для неудачной авторизации
                }

            }
            catch (Exception ex)
            {
                ApiResponse = $"Error: {ex.Message}";
                MessageBox.Show(ApiResponse);
            }
        }

        private void SelectPatientWindow()
        {
            AuthorizeADWindow ad = new AuthorizeADWindow();
            ad.Show();
        }
    }
}
