using Screenmate.Classe;
using Screenmate.Control;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Screenmate.Module.Calendar
{
    public partial class AddEvent : Window
    {
        bool ilestcheck = false;
        DaysControl daysControl;
        private List<EventSave> EventDate = new List<EventSave>();
        //private List<string> EventDate;
        private string DateS;

        public AddEvent(DaysControl Item,string Edate, string Etemp)
        {
            InitializeComponent();
            daysControl = Item;
            Date.Content = Edate;
            DateS=Etemp;
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
                    daysControl.Flag.Visibility = Visibility.Visible;
                    daysControl.content.Content = Type.Text;
                    string contain = Type.Text;
                    string Event = DateS;
                    bool Repeat = true;
                    Save(contain, Event, Repeat);
                }
            }
            else
            {
                daysControl.Flag.Visibility= Visibility.Visible;
                daysControl.content.Content = Type.Text;
                string contain = Type.Text;
                string Event = DateS;
                bool Repeat = false;
                Save(contain, Event, Repeat);
            }
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save(string contenue,string date, bool frequence)
        {
            string appDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string Savefile = System.IO.Path.Combine(appDirectory, "Calendar.dat");

            List<EventSave> existingEvents = EventSave.LoadEventSave(Savefile);

            //EventDate.Add(new EventSave
            existingEvents.Add(new EventSave
            {
                Content = contenue,
                Annuel = frequence,
                Date = date
            });

            EventSave.SaveEventSave(existingEvents, Savefile);
        }
    }
}
