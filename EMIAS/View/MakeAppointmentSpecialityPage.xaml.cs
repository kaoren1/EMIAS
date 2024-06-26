using System;
using System.Windows.Controls;
using System.Windows.Input;
using EMIAS.ViewModel.Helpers;
using MaterialDesignThemes.Wpf;
using EMIAS.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EMIAS.cards;
using EMIAS.Model;

namespace EMIAS.View
{
    public partial class MakeAppointmentSpecialityPage : Page
    {
        public MakeAppointmentSpecialityPage()
        {
            InitializeComponent();
            DataContext = new MakeAppointmentSpecialityViewModel();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MakeAppointmentPage());
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
