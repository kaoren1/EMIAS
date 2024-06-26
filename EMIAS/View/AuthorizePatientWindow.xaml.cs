﻿using EMIAS.View;
using EMIAS.ViewModel;
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

namespace EMIAS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthorizePatientWindow : Window
    {
        public AuthorizePatientWindow()
        {
            InitializeComponent();
            DataContext = new AuthorizePatientViewModel();
        }

        private void SelectADWindow_Click(object sender, RoutedEventArgs e)
        {
            AuthorizeADWindow authorizeADWindow = new AuthorizeADWindow();
            authorizeADWindow.Show();
            this.Close();
        }
    }
}
