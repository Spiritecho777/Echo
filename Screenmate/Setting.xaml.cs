using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace Screenmate
{
    public partial class Setting : Window
    {
        public static List varpath;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public Setting()
        {
            InitializeComponent();
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            listPath.Items.Remove(listPath.SelectedItem);
            string Savefile = (path + "/Path.txt");
            StreamWriter sw = new StreamWriter(Savefile);
            for (int i = 0; i < listPath.Items.Count; i++)
            {
                sw.WriteLine(listPath.Items[i].ToString());
            }
            sw.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var replace = Path.Text.Replace(@"\", "/");
            replace = replace.Replace(@"""", "");
            listPath.Items.Add(replace);
            string Savefile = (path + "/Path.txt");
            StreamWriter sw = new StreamWriter(Savefile);
            for (int i = 0; i < listPath.Items.Count; i++)
            {
                sw.WriteLine(listPath.Items[i].ToString());
            }
            sw.Close();
            Path.Text = String.Empty;
        }
    }
}
