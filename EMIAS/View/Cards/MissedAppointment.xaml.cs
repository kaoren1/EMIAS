using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class MissedAppointment : UserControl
    {
        public MissedAppointment(Appointment appointment)
        {
            InitializeComponent();
            DataContext = appointment;
        }
    }
}
