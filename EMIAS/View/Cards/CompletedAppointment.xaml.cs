using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class CompletedAppointment : UserControl
    {
        public CompletedAppointment(Appointment appointment)
        {
            InitializeComponent();
            DataContext = appointment;
        }
    }
}
