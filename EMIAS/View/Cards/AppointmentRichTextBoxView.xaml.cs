using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class AppointmentRichTextBoxView : UserControl
    {
        public AppointmentRichTextBoxView(AppointmentRichTextBox appointmentRichTextBox)
        {
            InitializeComponent();
            DataContext = appointmentRichTextBox;
        }
    }
}
