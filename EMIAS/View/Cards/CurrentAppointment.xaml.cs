using EMIAS.Model;
using System.Windows.Controls;

namespace EMIAS.View.Cards
{
    public partial class CurrentAppointment : UserControl
    {
        public CurrentAppointment(Appointment appointment)
        {
            InitializeComponent();
            DataContext = appointment;
        }
    }
}
