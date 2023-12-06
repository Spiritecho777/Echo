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
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void ChangeSM_Click(object sender, RoutedEventArgs e)
        {
            ChangeSM changeSM = new ChangeSM();
            changeSM.Show();
            this.Hide();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Option.initPath.Show();
            this.Hide();
        }

        private void EditVoc_Click(object sender, RoutedEventArgs e)
        {
            EditVoc Editvoc = new EditVoc();
            Editvoc.Show();
            this.Hide();
        }

        private void Rappel_Click(object sender, RoutedEventArgs e)
        {
            Calendrier calendar = new Calendrier();
            calendar.Show();
            this.Hide();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
