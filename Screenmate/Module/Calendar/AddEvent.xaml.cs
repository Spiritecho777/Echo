using Screenmate.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Screenmate.Module.Calendar
{
    public partial class AddEvent : Window
    {
        bool ilestcheck = false;
        DaysControl daysControl;

        public AddEvent(DaysControl Item,string Edate)
        {
            InitializeComponent();
            daysControl = Item;
            Date.Content = Edate;
        }

        private void Periodic_Check(object sender, RoutedEventArgs e)
        {
            ilestcheck=true;
            Frequence.IsEnabled = true;
        }

        private void Periodic_Uncheck(object sender , RoutedEventArgs e)
        {
            ilestcheck=false;
            Frequence.IsEnabled = false;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ilestcheck == true)
            {
                if (Frequence.SelectedIndex == 0)
                {
                    //Quotidien

                }

                if (Frequence.SelectedIndex == 1)
                {
                    //Hebdomadaire

                }

                if (Frequence.SelectedIndex == 2)
                {
                    //Mensuel

                }

                if(Frequence.SelectedIndex == 3)
                {
                    //Annuel

                }
            }
            else
            {
                daysControl.Flag.Visibility= Visibility.Visible;
            }
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
