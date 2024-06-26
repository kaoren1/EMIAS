using System;
using System.Windows.Controls;
using System.Windows.Input;
using EMIAS.ViewModel.Helpers;
using MaterialDesignThemes.Wpf;
using EMIAS.ViewModel;
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

    public partial class MakeAppointmentSpecialityPage2 : Page
    {
        public MakeAppointmentSpecialityPage2()
        {
            InitializeComponent();
            DataContext = new MakeAppointmentSpecialityViewModel();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpeciality = (sender as ListBox).SelectedItem;
            NavigationService.Navigate(new MakeAppointmentSpecialityPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MakeAppointmentPage());
        }
    }
}

