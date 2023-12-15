using Screenmate.Control;
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
using System.Windows.Shapes;

namespace Screenmate.Module
{
    public partial class Calendrier : Window
    {
        public Calendrier()
        {
            InitializeComponent();
        }

        private void Calendrier_Load(object sender, EventArgs e)
        {
            DisplayDays();
        }

        private void DisplayDays()
        {
            DateTime now = DateTime.Now;

            DateTime startMonth = new DateTime(now.Year, now.Month, 1);
            int days =DateTime.DaysInMonth(now.Year, now.Month);
            int daysWeek =Convert.ToInt32(startMonth.DayOfWeek.ToString("d"))+1;

            for (int i =1; i <= daysWeek; i++) 
            {
                BlankDaysControl blanckDays = new BlankDaysControl();
                blanckDays.Margin=new Thickness(1,1,1,0);
                dayContainer.Children.Add(blanckDays);
            }

            for (int i = 1; i <= days; i++) 
            {
                DaysControl daysControl = new DaysControl();
                daysControl.Margin=new Thickness(1,1,1,0);
                daysControl.days(i);
                dayContainer.Children.Add(daysControl);
            }
        }

        public DateTime? SelectedDate { get; private set; }

        private void OptionsContext_Calendar(object sender, MouseButtonEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                var itemUnderMouse = e.OriginalSource as FrameworkElement;
                //bookmarkUnderMouse = itemUnderMouse?.DataContext as Bookmark;

                DateTime? dateClicked = calendar.SelectedDate;

                if (dateClicked.HasValue)
                {
                    calendar.SelectedDate = dateClicked.Value;
                    ContextMenu contextMenu = Resources["CalendarContextMenu"] as ContextMenu;
                    contextMenu.PlacementTarget = calendar;
                    contextMenu.IsOpen = true;
                    e.Handled = true;
                }
            }
        }

        private void AddRappel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelRappel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifyRappel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
