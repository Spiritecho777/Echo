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
