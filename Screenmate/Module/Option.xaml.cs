using Screenmate.Module;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Screenmate.Module
{
    public partial class Option : Window
    {
        string truepath = "";
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public bool allowedMove = false;
        
        public static InitPath initPath = new InitPath();
        public static ChangeSM Changesm = new ChangeSM();

        Setting Setting = new Setting();
        Procedure Procedure = new Procedure();
        public Option()
        {
            InitializeComponent();
            Vocal.Background = Properties.Settings.Default.VocalEnabled ? Brushes.Blue : Brushes.Red;
            Move.Background = allowedMove ? Brushes.Blue : Brushes.Red;
            //string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string Savefile = System.IO.Path.Combine(appDirectory, "Path.txt");
            if (File.Exists(Savefile))
            {
                StreamReader sr = new StreamReader(Savefile);
                List<string> lines = new List<string>();

                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    else
                    {
                        lines.Add(line);
                    }
                }
                sr.Close();
                for (int i = 0; i < lines.Count; i++)
                {
                    initPath.listPath.Items.Add(lines[i]);
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(Savefile)) { }
            }
        }

        #region gros bouton
        private void WorkApp_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < initPath.listPath.Items.Count; i++)
            {
                truepath = initPath.listPath.Items[i].ToString();
                System.Diagnostics.Process.Start(truepath);
            }
            if (allowedMove == true)
            {
                MainWindow.timer.Start();
                MainWindow.autonomyTimer.Start();
            }
            this.Hide();
        }

        private void WorkAppClose_Click(object sender, RoutedEventArgs e)
        {
            for (int i2 = 0; i2 < initPath.listPath.Items.Count; i2++)
            {
                string pathn = initPath.listPath.Items[i2].ToString();
                int pathc1 = pathn.LastIndexOf(".");
                int pathc2 = pathn.LastIndexOf("/");
                int pathc = pathc1 - pathc2 - 1;
                string appname = pathn.Substring(pathc2 + 1, pathc);
                foreach (var process in Process.GetProcessesByName(appname))
                {
                    process.Kill();
                }
            }

            if (allowedMove == true)
            {
                MainWindow.timer.Start();
                MainWindow.autonomyTimer.Start();
            }
            this.Hide();
        }

        private void Tuto_Click(object sender, RoutedEventArgs e)
        {
            Procedure.Show();
            this.Hide();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Setting.Show();
            this.Hide();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Changesm.Hide();
            Setting.Hide();
            Procedure.Hide();
            this.Hide();
        }

        private void KillApp_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /*
        private void ChangeSM_Click(object sender, RoutedEventArgs e)
        {
            ChangeSM Changesm =new ChangeSM();
            Changesm.Show();
            this.Hide();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Setting.Show();
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
        */
        #endregion

        #region petit bouton
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            allowedMove = !allowedMove;
            Move.Background = allowedMove ? Brushes.Blue : Brushes.Red;
        }

        private void Vocal_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VocalEnabled = !Properties.Settings.Default.VocalEnabled;
            Properties.Settings.Default.Save();
            Vocal.Background = Properties.Settings.Default.VocalEnabled ? Brushes.Blue : Brushes.Red;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Numero de version actuel : "+ System.Windows.Forms.Application.ProductVersion);
        }
        #endregion

        #region méthode annexe
        public void StartDay()
        {
            for (int i = 0; i < initPath.listPath.Items.Count; i++)
            {
                truepath = initPath.listPath.Items[i].ToString();
                System.Diagnostics.Process.Start(truepath);
            }
        }

        public void EndDay()
        {
            for (int i2 = 0; i2 < initPath.listPath.Items.Count; i2++)
            {
                string pathn = initPath.listPath.Items[i2].ToString();
                int pathc1 = pathn.IndexOf(".");
                int pathc2 = pathn.LastIndexOf("/");
                int pathc = pathc1 - pathc2 - 1;
                string appname = pathn.Substring(pathc2 + 1, pathc);

                foreach (var process in Process.GetProcessesByName(appname))
                {
                    process.Kill();
                }
            }
        }

        public void Explo()
        {
            Procedure.Show();
        }

        public void ChangeC()
        {
            ChangeSM Changesm = new ChangeSM();
            Changesm.Show();
        }

        public void EditCmd()
        {
            EditVoc Editvoc = new EditVoc();
            Editvoc.Show();
        }

        public void param()
        {
            Setting.Show();
        }

        public void Fermeture()
        {
            Environment.Exit(0);
        }

        public void MoveChangeColor()
        {
            Move.Background = allowedMove ? Brushes.Blue : Brushes.Red;
        }
        #endregion

    }
}
