using EMIAS.cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EMIAS.ViewModel.Helpers;

namespace EMIAS.ViewModel
{
    public class DayOfMonthViewModel : INotifyPropertyChanged
    {
        private DateTime _day;
        private ObservableCollection<DateTime> _daysOfWeek;
        public DateTime Day
        {
            get { return _day; }
            set
            {
                _day = value;
                OnPropertyChanged(nameof(Day));
                OnPropertyChanged(nameof(DayOfWeek));
                OnPropertyChanged(nameof(IsToday));
            }
        }
        public DayOfWeek DayOfWeek => Day.DayOfWeek;

        public bool IsToday => Day.Date == DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MakeAppointmentSpecialityViewModel : INotifyPropertyChanged
    {

        private DateTime _selectedDate;
        private ObservableCollection<DateTime> _daysOfMonth;
        private string _selectedTimeSlot;
        private ObservableCollection<string> _timeSlots; // Переменная для временных слотов


        public event EventHandler SelectedDateChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SpecialistSquareViewModel> _specialists;
        public ObservableCollection<SpecialistSquareViewModel> Specialists
        {
            get { return _specialists; }
            set
            {
                _specialists = value;
                OnPropertyChanged(nameof(Specialists));
            }
        }
        public MakeAppointmentSpecialityViewModel()
        {
            PreviousMonthCommand = new RelayCommand(PreviousMonth, CanExecutePreviousMonth);
            NextMonthCommand = new RelayCommand(NextMonth);
            BackCommand = new RelayCommand(Back);
            SignUpCommand = new RelayCommand(SignUp);
            SelectedDate = DateTime.Now;
            LoadDaysOfMonth();
            LoadTimeSlots();
        }

        public void SelectDate(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }

        public ICommand PreviousMonthCommand { get; private set; }
        public ICommand NextMonthCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand SignUpCommand { get; private set; }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    OnPropertyChanged(nameof(CurrentMonth));
                    OnPropertyChanged(nameof(CanExecutePreviousMonth));
                    LoadDaysOfMonth(); // При изменении даты загружаем новые дни месяца
                    ((RelayCommand)PreviousMonthCommand).RaiseCanExecuteChanged(); // Update command execution state
                }
            }
        }

        public class DayOfMonthViewModel
        {
            public DateTime Day { get; set; }
            public bool IsPastDate => Day < DateTime.Today;
        }

        public string CurrentMonth => SelectedDate.ToString("MMMM yyyy");

        public ObservableCollection<DateTime> DaysOfMonth
        {
            get { return _daysOfMonth; }
            set
            {
                _daysOfMonth = value;
                OnPropertyChanged(nameof(DaysOfMonth));
            }
        }

        private void LoadDaysOfMonth()
        {
            int year = SelectedDate.Year;
            int month = SelectedDate.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            int startDay;
            if (year == DateTime.Now.Year && month == DateTime.Now.Month)
            {
                // If the selected date is in the current month, start from today's date
                startDay = DateTime.Now.Day;
            }
            else
            {
                // Otherwise, start from the first of the month
                startDay = 1;
            }

            List<DayOfMonthViewModel> days = new List<DayOfMonthViewModel>();
            for (int day = startDay; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(year, month, day);
                days.Add(new DayOfMonthViewModel { Day = date });
            }

            // Update the ObservableCollection
            DaysOfMonth = new ObservableCollection<DateTime>(days.Select(vm => vm.Day));
        }




        public ObservableCollection<string> TimeSlots // Использование ObservableCollection<string> для временных слотов
        {
            get { return _timeSlots; }
            set
            {
                _timeSlots = value;
                OnPropertyChanged(nameof(TimeSlots));
            }
        }

        public string SelectedTimeSlot
        {
            get { return _selectedTimeSlot; }
            set
            {
                if (_selectedTimeSlot != value)
                {
                    _selectedTimeSlot = value;
                    OnPropertyChanged(nameof(SelectedTimeSlot));
                }
            }
        }

        private void LoadTimeSlots()
        {
            var startTime = new DateTime(1, 1, 1, 8, 0, 0); // 8:00 AM
            var endTime = new DateTime(1, 1, 1, 20, 0, 0);  // 9:00 PM

            TimeSlots = new ObservableCollection<string>();
            while (startTime <= endTime)
            {
                TimeSlots.Add(startTime.ToString("HH:mm"));
                startTime = startTime.AddMinutes(10);
            }
        }


        private bool CanExecutePreviousMonth(object obj)
        {
            // Check if the previous month is not in the past
            DateTime previousMonth = SelectedDate.AddMonths(-1);
            return previousMonth >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        private void PreviousMonth(object obj)
        {
            if (CanExecutePreviousMonth(null))
            {
                SelectedDate = SelectedDate.AddMonths(-1);
            }
        }

        private void NextMonth(object obj)
        {
            SelectedDate = SelectedDate.AddMonths(1);
        }

        private void Back(object obj)
        {
            // Действие при нажатии кнопки "Назад"
        }

        private void SignUp(object obj)
        {
            // Действие при нажатии кнопки "Записаться на прием"
            if (!string.IsNullOrEmpty(SelectedTimeSlot))
            {
                // Здесь можете добавить логику для записи на прием, используя SelectedDate и SelectedTimeSlot
                MessageBox.Show($"Вы записаны на прием {SelectedDate.ToShortDateString()} в {SelectedTimeSlot}", "Успешная запись на прием", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите время для записи на прием.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedDate))
            {
                SelectedDateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}