using Screenmate.Classe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Screenmate
{
    public partial class EditVoc : Window
    {
        private List<CmdVoc> vocalCommands = new List<CmdVoc>();
        public EditVoc()
        {
            InitializeComponent();

            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath = Path.Combine(appDirectory, "Cmdvoc.dat");

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            if (!File.Exists(filePath))
            {
                CmdVoc Cmdvoc = new CmdVoc();
                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Déplacement du Compagnon",
                    Phrase = new List<string> { "déplace-toi", "bouge" },
                    ActionMethodName = "Move"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Arrêt du Compagnon",
                    Phrase = new List<string> { "Arrête", "Stop", },
                    ActionMethodName = "Stop"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Liste des commandes",
                    Phrase = new List<string> { "Donne moi la liste des commandes" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Lancements des logiciels",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Fermetures des logiciels",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Explorateurs de fichiers",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Changement de compagnon",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Edition de commandes vocales",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Paramétrage des chemins d'accèes",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });

                vocalCommands.Add(new CmdVoc
                {
                    Nom = "Quitter l'application",
                    Phrase = new List<string> { "" },
                    ActionMethodName = "Get-CmdList"
                });


                Cmdvoc.SaveCmdVoc(vocalCommands, filePath);
            }
            else
            {
                vocalCommands = CmdVoc.LoadCmdVoc(filePath);
            }

            PopulateList(vocalCommands);
        }
        private void PopulateList(List<CmdVoc> vocalCommands)
        {
            listAction.SelectionChanged -= SelectedAction;

            listAction.SelectedIndex = -1;

            listCommande.Items.Clear();

            listAction.Items.Clear();

            foreach (var cmdVoc in vocalCommands)
            {
                listAction.Items.Add(cmdVoc.Nom);
            }

            listAction.SelectionChanged += SelectedAction;
        }

        private void SelectedAction(object sender, SelectionChangedEventArgs e)
        {
            listCommande.Items.Clear();

            string selectedAct=listAction.SelectedItem.ToString();
            if (selectedAct != null)
            {
                // Recherchez l'action correspondante dans vocalCommands
                CmdVoc selectedCommand = vocalCommands.FirstOrDefault(cmd => cmd.Nom == selectedAct);

                if (selectedCommand != null)
                {
                    List<string> phrases = selectedCommand.Phrase;

                    foreach (var phrase in phrases)
                    {
                        listCommande.Items.Add(phrase);
                    }
                }
            }
        }

        private void Add_Click (object sender, RoutedEventArgs e)
        {
            CmdVoc Cmdvoc = new CmdVoc();
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath = Path.Combine(appDirectory, "Cmdvoc.dat");

            int selectedIndex = listAction.SelectedIndex;


            if (selectedIndex >= 0 && selectedIndex < vocalCommands.Count && !string.IsNullOrWhiteSpace(AddField.Text))
            {
                CmdVoc selectedAction = vocalCommands[selectedIndex];

                if (!vocalCommands.Any(cmdVoc => cmdVoc.Phrase.Any(phrase => string.Equals(phrase, AddField.Text, StringComparison.OrdinalIgnoreCase))))
                {
                    selectedAction.Phrase.Add(AddField.Text);

                    Cmdvoc.SaveCmdVoc(vocalCommands, filePath);

                    PopulateList(vocalCommands);
                }
                else
                {
                    System.Windows.MessageBox.Show("La commande est déja présente dans une action");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Veuillez selectionner une action");
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            CmdVoc Cmdvoc = new CmdVoc();
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath = Path.Combine(appDirectory, "Cmdvoc.dat");

            int selectedIndex = listCommande.SelectedIndex;

            if (selectedIndex >= 0)
            {
                int selectedActionIndex = listAction.SelectedIndex;

                if (selectedActionIndex >= 0 && selectedActionIndex < vocalCommands.Count)
                {
                    CmdVoc selectedAction = vocalCommands[selectedActionIndex];

                    if (selectedIndex < selectedAction.Phrase.Count)
                    {
                        selectedAction.Phrase.RemoveAt(selectedIndex);

                        Cmdvoc.SaveCmdVoc(vocalCommands, filePath);

                        PopulateList(vocalCommands);
                    }
                }
            }
        }

        private void Back_Click (object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Afin de mettre à jour la liste de commandes veuillez relancer l'application");
            this.Close();
        }
    }
}
