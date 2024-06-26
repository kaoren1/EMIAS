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
    public partial class MedicalCardAnalysPage : Page, INotifyPropertyChanged
    {
        private HttpClient httpClient;
        private ObservableCollection<Analysis> _analyses;
        private string _patientOms; // Поле для хранения номера ОМС пациента

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Analysis> Analyses
        {
            get { return _analyses; }
            set
            {
                _analyses = value;
                OnPropertyChanged();
            }
        }

        public MedicalCardAnalysPage(string patientOms)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            Analyses = new ObservableCollection<Analysis>();
            _patientOms = patientOms; // Сохраняем номер ОМС
            LoadAnalyses(_patientOms); // Загружаем анализы для указанного ОМС
        }

        private async void LoadAnalyses(string oms)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/Analyses/{oms}");

                if (response.IsSuccessStatusCode)
                {
                    var analysesJson = await response.Content.ReadAsStringAsync();
                    var analyses = JsonConvert.DeserializeObject<ObservableCollection<Analysis>>(analysesJson);

                    Analyses.Clear(); // Очищаем текущие данные

                    foreach (var analysis in analyses)
                    {
                        Analyses.Add(analysis);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки анализов.");
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
                if (stackPanel.DataContext is Analysis analysis)
                {
                    // Показываем подробную информацию в текстовых блоках
                    textLocation.Text = analysis.DoctorWorkAddress;
                    textDate.Text = analysis.AnalysisDate.ToString("dd MMMM yyyy");

                    // Загружаем и отображаем документ в формате RTF
                    await LoadRtfDocument(analysis.IdAnalysis);
                }
            }
        }

        private async Task LoadRtfDocument(int analysisId)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/AnalysisDocuments/{analysisId}");

                if (response.IsSuccessStatusCode)
                {
                    var documentJson = await response.Content.ReadAsStringAsync();
                    var document = JsonConvert.DeserializeObject<AnalysisDocument>(documentJson);

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

    public class Analysis
    {
        public int IdAnalysis { get; set; }
        public long OMS { get; set; }
        public string DoctorName { get; set; }
        public string DoctorWorkAddress { get; set; }
        public DateTime AnalysisDate { get; set; }
    }

    public class AnalysisDocument
    {
        public int IdAnalysisDocument { get; set; }
        public int AnalysisId { get; set; }
        public string Rtf { get; set; }
    }
}
