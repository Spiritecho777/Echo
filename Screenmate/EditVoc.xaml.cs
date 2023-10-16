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

                Cmdvoc.SaveCmdVoc(vocalCommands, filePath);
            }
            else
            {
                // Le fichier existe, chargez la structure de dossier depuis le fichier
                //CmdVoc Cmdvoc = new CmdVoc();
                vocalCommands = CmdVoc.LoadCmdVoc(filePath);
            }

            PopulateList(vocalCommands);
        }
        private void PopulateList(List<CmdVoc> vocalCommands)
        {
            foreach (var cmdVoc in vocalCommands)
            {
                // Ajoutez les éléments à la liste d'actions
                listAction.Items.Add(cmdVoc.Nom);
            }
        }

        private void SelectedAction(object sender, SelectionChangedEventArgs e)
        {
            listCommande.Items.Clear();


            int selectedIndex = listAction.SelectedIndex;

            // Vérifiez si un élément est sélectionné
            if (selectedIndex >= 0 && selectedIndex < vocalCommands.Count)
            {
                // Récupérez les phrases vocales associées à l'action sélectionnée
                List<string> phrases = vocalCommands[selectedIndex].Phrase;

                // Ajoutez les phrases vocales à la liste des commandes
                foreach (var phrase in phrases)
                {
                    listCommande.Items.Add(phrase);
                }
            }
        }
    }
}
