using EMIAS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EMIAS.View
{
    public partial class MedicalCardAppointmentPage : Page, INotifyPropertyChanged
    {
        private HttpClient httpClient;
        private ObservableCollection<Appointment> _appointments;
        private string _patientOms; // Поле для хранения номера ОМС пациента

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Appointment> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                OnPropertyChanged();
            }
        }


        public MedicalCardAppointmentPage(string patientOms)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            Appointments = new ObservableCollection<Appointment>();
            _patientOms = patientOms; // Сохраняем номер ОМС
            LoadAppointments(_patientOms); // Загружаем приемы для указанного ОМС
        }

        private async void LoadAppointments(string oms)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/Appointments/{oms}");

                if (response.IsSuccessStatusCode)
                {
                    var appointmentsJson = await response.Content.ReadAsStringAsync();
                    var appointments = JsonConvert.DeserializeObject<ObservableCollection<Appointment>>(appointmentsJson);

                    Appointments.Clear(); // Очищаем текущие данные

                    foreach (var appointment in appointments)
                    {
                        Appointments.Add(appointment);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки приемов.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Обработчик клика на StackPanel
        private async void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is StackPanel stackPanel)
            {
                if (stackPanel.DataContext is Appointment appointment)
                {
                    // Показываем подробную информацию в текстовых блоках
                    textDoctorName.Text = appointment.DoctorName;
                    textLocation.Text = appointment.DoctorWorkAddress;
                    textDate.Text = appointment.AppointmentDate.ToString();

                    // Загружаем и отображаем документ в формате RTF
                    await LoadRtfDocument(appointment.ID_Appointment);
                }
            }
        }

        private async Task LoadRtfDocument(int appointmentId)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/AppointmentDocuments/{appointmentId}");

                if (response.IsSuccessStatusCode)
                {
                    var documentJson = await response.Content.ReadAsStringAsync();
                    var document = JsonConvert.DeserializeObject<AppointmentDocument>(documentJson);

                    using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(document.Rtf)))
                    {
                        TextRange range = new TextRange(DocumentsRichTextBox.Document.ContentStart, DocumentsRichTextBox.Document.ContentEnd);
                        range.Load(stream, DataFormats.Rtf);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки документа.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
    public class Appointment
    {
        public int ID_Appointment { get; set; }
        public string DoctorName { get; set; }
        public string DoctorWorkAddress { get; set; }
        public DateTime AppointmentDate { get; set; }
        
    }
    public class AppointmentDocument
    {
        public int ID_AppointmentDocument { get; set; }
        public int Appointment_ID { get; set; }
        public string Rtf { get; set; }
    }


}
