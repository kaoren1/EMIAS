using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMIAS.View
{
    public partial class MeddicalCardResearchPage : Page

    {
        private HttpClient httpClient;
        private ObservableCollection<Research> _researches;
        private string _patientOms;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Research> Researches
        {
            get { return _researches; }
            set
            {
                _researches = value;
                OnPropertyChanged();
            }
        }
        public MeddicalCardResearchPage(string patientOms)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            Researches = new ObservableCollection<Research>();
            _patientOms = patientOms; // Сохраняем номер ОМС
            LoadResearches(_patientOms); // Загружаем исследования для указанного ОМС
        }
        private async void LoadResearches(string oms)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/Researches/{oms}");

                if (response.IsSuccessStatusCode)
                {
                    var researchesJson = await response.Content.ReadAsStringAsync();
                    var researches = JsonConvert.DeserializeObject<ObservableCollection<Research>>(researchesJson);

                    Researches.Clear(); // Очищаем текущие данные

                    foreach (var research in researches)
                    {
                        Researches.Add(research);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки исследований.");
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
                if (stackPanel.DataContext is Research research)
                {
                    // Показываем подробную информацию в текстовых блоках
                    textResearchName.Text = research.ResearchName;
                    textDoctorName.Text = research.DoctorName;
                    textLocation.Text = research.DoctorWorkAddress;
                    textDate.Text = research.ResearchDate.ToString("dd MMMM yyyy");

                    // Загружаем и отображаем документ в формате RTF
                    await LoadRtfDocument(research.IdResearch);
                }
            }
        }

        private async Task LoadRtfDocument(int researchId)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://localhost:5181/api/ResearchDocuments/{researchId}");

                if (response.IsSuccessStatusCode)
                {
                    var documentJson = await response.Content.ReadAsStringAsync();
                    var document = JsonConvert.DeserializeObject<ResearchDocument>(documentJson);

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

    public class Research
    {
        public int IdResearch { get; set; }
        public long OMS { get; set; }
        public string ResearchName { get; set; }
        public string DoctorName { get; set; }
        public string DoctorWorkAddress { get; set; }
        public DateTime ResearchDate { get; set; }
    }

    public class ResearchDocument
    {
        public int IdResearchDocument { get; set; }
        public int ResearchId { get; set; }
        public string Rtf { get; set; }
    }
}