using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class AnalysisRichTextBoxView : UserControl
    {
        public AnalysisRichTextBoxView(AnalysisRichTextBox analysisRichTextBox)
        {
            InitializeComponent();
            DataContext = analysisRichTextBox;
        }
    }
}
