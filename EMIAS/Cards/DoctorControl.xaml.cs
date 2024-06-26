using EMIAS.Model;
using EMIAS.View;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace EMIAS.Cards
{
    /// <summary>
    /// Логика взаимодействия для DoctorControl.xaml
    /// </summary>
    public partial class DoctorControl : UserControl
    {
        public DoctorControl()
        {
            InitializeComponent();

           
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var speciality = this.DataContext as Speciality;
            if (speciality != null)
            {
                OpenSpecialityPage(speciality);
            }
        }
        private void OpenSpecialityPage(Speciality speciality)
        {
            var mainWindow = new MainPatientWindow();
            mainWindow.DataContext = this;
            mainWindow.Show();


            var frame = mainWindow.FindName("PageFrame") as Frame;
            frame.Navigate(new MakeAppointmentSpecialityPage());

            Application.Current.Windows[0]?.Close();
        }
    }
}
