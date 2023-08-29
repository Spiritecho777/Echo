using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Screenmate
{
    public partial class Procedure : Window
    {
        public string chemin = "";
        string Lastp;
        List<string> truePathList = new List<string>();

        public Procedure()
        {
            InitializeComponent();
            string savedFolderPath = Properties.Settings.Default.FolderPath;
            if (!string.IsNullOrEmpty(savedFolderPath) && Directory.Exists(savedFolderPath))
            {
                Path.Text = savedFolderPath;
                Lastp = Path.Text;
            }
            LoadFilesInDirectory();
        }

        private void LoadFilesInDirectory()
        {
            chemin = Path.Text;
            if (Directory.Exists(chemin))
            {
                string[] files = Directory.GetFiles(chemin);
                string[] directories = Directory.GetDirectories(chemin);

                listBox.Items.Clear();

                foreach (string directory in directories)
                {
                    if (!IsHidden(directory))
                    {
                        listBox.Items.Add("[Dossier] " + System.IO.Path.GetFileName(directory));
                    }
                }

                foreach (string file in files)
                {
                    if (!IsHidden(file))
                    {
                        listBox.Items.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedEntry = listBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedEntry))
            {
                string fullPath;
                if (selectedEntry.StartsWith("[Dossier] "))
                {
                    string folderName = selectedEntry.Replace("[Dossier] ", "");
                    fullPath = System.IO.Path.Combine(chemin, folderName);
                }
                else
                {
                    if (Search.Text == "")
                    {
                        fullPath = System.IO.Path.Combine(chemin, selectedEntry);
                    }
                    else
                    {
                        fullPath = truePathList[listBox.SelectedIndex];
                    }
                }

                if (File.Exists(fullPath))
                {
                    Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
                }
                else if (Directory.Exists(fullPath))
                {
                    Lastp = Path.Text;
                    Path.Text = fullPath;
                    Path.Text = Path.Text.Replace(@"\", "/");
                    LoadFilesInDirectory();
                }
            }
        }

        private void LastPath_Click(object sender, RoutedEventArgs e)
        {
            Path.Text = Lastp;
            Search.Text = "";
            truePathList.Clear();
            LoadFilesInDirectory();
        }

        private void Path_Key(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                Search.Text = "";
                truePathList.Clear();
                LoadFilesInDirectory();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string chemin = Path.Text.Trim();
            string searchText = Search.Text.Trim().ToLower();

            truePathList.Clear();

            if (searchText == "")
            {
                LoadFilesInDirectory();
            }
            else
            {
                if (Directory.Exists(chemin))
                {
                    listBox.Items.Clear();
                    SearchInDirectory(chemin, searchText);
                }
            }
        }

        private void SearchInDirectory(string directory, string searchText)
        {
            string[] files = Directory.GetFiles(directory);

            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file).ToLower();

                if (fileName.Contains(searchText))
                {
                    listBox.Items.Add(System.IO.Path.GetFileName(file));
                    truePathList.Add(file);
                }
            }

            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                SearchInDirectory(subDirectory, searchText);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FolderPath = chemin;
            Properties.Settings.Default.Save();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private bool IsHidden(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);
            return (attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
    }
}
