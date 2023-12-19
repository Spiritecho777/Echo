using Screenmate.Control;
using Screenmate.Module;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Screenmate.Module
{

    public partial class Calendrier : Window
    {
        int month, year;
        string Emandy, Eday, Edate;
        DaysControl Item;

        public Calendrier()
        {
            InitializeComponent();
        }

        private void Calendrier_Load(object sender, EventArgs e)
        {
            DisplayDays();
        }

        #region Layout Calendar
        private void DisplayDays()
        {
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            DateMY.Content = char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;

            DateTime startMonth = new DateTime(year, month, 1);
            int days =DateTime.DaysInMonth(year, month);
            int daysWeek =Convert.ToInt32(startMonth.DayOfWeek.ToString("d"))-1;
            for (int i =1; i <= daysWeek; i++) 
            {
                BlankDaysControl blanckDays = new BlankDaysControl();
                blanckDays.Margin=new Thickness(1,1,1,0);
                dayContainer.Children.Add(blanckDays);
            }

            for (int i = 1; i <= days; i++)
            {
                DaysControl daysControl = new DaysControl();
                daysControl.Margin = new Thickness(1, 1, 1, 0);
                daysControl.MouseRightButtonDown += OptionsContext_Calendar;
                daysControl.days(i);
                dayContainer.Children.Add(daysControl);
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            month--;
            if (month < 1)
            {
                month = 12;
                year--;
            }
            dayContainer.Children.Clear();
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            DateMY.Content = char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;

            DateTime startMonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int daysWeek = Convert.ToInt32(startMonth.DayOfWeek.ToString("d")) - 1;

            for (int i = 1; i <= daysWeek; i++)
            {
                BlankDaysControl blanckDays = new BlankDaysControl();
                blanckDays.Margin = new Thickness(1, 1, 1, 0);
                dayContainer.Children.Add(blanckDays);
            }

            for (int i = 1; i <= days; i++)
            {
                DaysControl daysControl = new DaysControl();
                daysControl.Margin = new Thickness(1, 1, 1, 0);
                daysControl.MouseRightButtonDown += OptionsContext_Calendar;
                daysControl.days(i);
                dayContainer.Children.Add(daysControl);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            dayContainer.Children.Clear();
            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            DateMY.Content = char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;

            DateTime startMonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int daysWeek = Convert.ToInt32(startMonth.DayOfWeek.ToString("d")) - 1;

            for (int i = 1; i <= daysWeek; i++)
            {
                BlankDaysControl blanckDays = new BlankDaysControl();
                blanckDays.Margin = new Thickness(1, 1, 1, 0);
                dayContainer.Children.Add(blanckDays);
            }

            for (int i = 1; i <= days; i++)
            {
                DaysControl daysControl = new DaysControl();
                daysControl.Margin = new Thickness(1, 1, 1, 0);
                daysControl.MouseRightButtonDown += OptionsContext_Calendar;
                daysControl.days(i);
                dayContainer.Children.Add(daysControl);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void OptionsContext_Calendar(object sender, MouseButtonEventArgs e)
        {
            if (sender is DaysControl calendarItem)
            {
                var itemUnderMouse = e.OriginalSource as FrameworkElement;
                if (!calendarItem.IsSelected)
                {
                    calendarItem.IsSelected = true;
                }

                calendarItem.SelectedItem = calendarItem;
                ContextMenu contextMenu = Resources["CalendarContextMenu"] as ContextMenu;
                contextMenu.PlacementTarget = calendarItem;
                contextMenu.IsOpen = true;
                e.Handled = true;
                Eday = calendarItem.NumberDays.Content.ToString();
                Emandy=DateMY.Content.ToString();
                Edate=Eday + " " + Emandy;
                Item = calendarItem;
            }
        }

        private void AddRappel_Click(object sender, RoutedEventArgs e)
        {
            Calendar.AddEvent Event = new Calendar.AddEvent(Item,Edate);
            Event.Show();
        }

        private void DelRappel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
