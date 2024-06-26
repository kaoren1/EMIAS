using System.Windows.Controls;
using EMIAS.ViewModel;
using EMIAS.Cards;
using System.Windows;
using EMIAS.Model;
using System.Collections.Generic;
using System;

namespace EMIAS.View
{
    public partial class MakeAppointmentPage : Page
    {
        private MakeAppointmentVM viewModel;

        public MakeAppointmentPage()
        {
            InitializeComponent();
            viewModel = new MakeAppointmentVM();
            DataContext = viewModel;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainPatientWindow();
            mainWindow.DataContext = this;
            mainWindow.Show();


            var frame = mainWindow.FindName("PageFrame") as Frame;
            frame.Navigate(new MakeAppointmentPage2());

            Application.Current.Windows[0]?.Close();
        }
    }
}
