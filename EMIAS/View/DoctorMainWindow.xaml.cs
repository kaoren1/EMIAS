﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EMIAS.ViewModel;

namespace EMIAS.View
{
    public partial class DoctorMainWindow : Window
    {
        int id = 1; //типо получил айди доктора
        public DoctorMainWindow()
        {
            InitializeComponent();
            DataContext = new DoctorMainWindowViewModel(id);
        }

        #region TitleBar
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
                screenmode.Source = new BitmapImage(new Uri("../Images/fullscreen_exit.png", UriKind.Relative));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (WindowState == WindowState.Maximized)
                {
                    var mousePos = PointToScreen(e.GetPosition(this));
                    WindowState = WindowState.Normal;
                    Left = mousePos.X - (ActualWidth / 2);
                    Top = mousePos.Y - (30 / 2);
                }
                DragMove();
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized && WindowStyle == WindowStyle.None)
            {
                screenmode.Source = new BitmapImage(new Uri("../Images/fullscreen_exit.png", UriKind.Relative));
            }
            else
            {
                screenmode.Source = new BitmapImage(new Uri("../Images/FullScreen.png", UriKind.Relative));
            }
        }
        #endregion
    }
}
