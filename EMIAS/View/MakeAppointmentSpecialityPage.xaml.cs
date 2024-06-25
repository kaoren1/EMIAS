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

        private void DaySquareControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var daySquareControl = sender as DaySquareControl;
                DateTime currentDate = daySquareControl.Date;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu contextMenu = this.ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = this;
                    contextMenu.IsOpen = true;
                }
            }
        }

    }
}
