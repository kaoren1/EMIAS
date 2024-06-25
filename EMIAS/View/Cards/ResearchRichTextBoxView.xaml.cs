using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class ResearchRichTextBoxView : UserControl
    {
        public ResearchRichTextBoxView(ResearchRichTextBox researchRichTextBox)
        {
            InitializeComponent();
            DataContext = researchRichTextBox;
        }
    }
}
