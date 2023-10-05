using Screenmate.Classe;
using System;
using System.IO;
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
using Path = System.IO.Path;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;

namespace Screenmate
{
    public partial class ChangeSM : Window
    {
        private FolderStructure folderStructure;
        public ChangeSM()
        {
            InitializeComponent();

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "Sprite.dat");

            //folderStructure = LoadFolderStructure();

            //if (folderStructure ==null || folderStructure.Folders == null)
                if (!File.Exists(filePath))
                {
                // Initialiser folderStructure à une nouvelle instance si elle est null
                folderStructure = new FolderStructure
                {
                    Folders = new Dictionary<string, Dictionary<string, List<string>>>
                    {
                        // Ajoutez le dossier par défaut ici
                        {
                            "Renard (Défaut)",
                            new Dictionary<string, List<string>>
                            {
                                {
                                     "AnimationIdle",
                                        new List<string>
                                        {
                                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Idle0.png",
                                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Idle1.png"
                                        }
                                },
                                {
                                    "AnimationSleep",
                                    new List<string>
                                    {
                                        "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Sleep0.png",
                                        "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Sleep1.png"
                                    }
                                },
                                {
                                    "AnimationWalk",
                                    new List<string>
                                    {
                                        "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Walk0.png",
                                        "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Walk.png"
                                    }
                                }
                            }
                        }
                    }
                };

                FileManager fileManager = new FileManager();
                fileManager.SaveFolderStructure(folderStructure, filePath);
            }
            else
            {
                // Le fichier existe, chargez la structure de dossier depuis le fichier
                FileManager fileManager = new FileManager();
                folderStructure = fileManager.LoadFolderStructure(filePath);
            }

            /*folderStructure = LoadFolderStructure();

            if (folderStructure == null)
            {
                // Initialiser folderStructure à une nouvelle instance si elle est null
                folderStructure = new FolderStructure();
            }*/

            PopulateTreeView(folderStructure);

        }

        private FolderStructure LoadFolderStructure()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");
            if (File.Exists(filePath))
            {
                // Le fichier existe, chargez la structure de dossier depuis le fichier
                FileManager fileManager = new FileManager();
                return fileManager.LoadFolderStructure(filePath);
            }
            else
            {
                return new FolderStructure();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FolderName.Text != "Nom du compagnon a ajouter" && !string.IsNullOrWhiteSpace(FolderName.Text) && !folderStructure.Folders.ContainsKey(FolderName.Text))
            {
                // Créez le nouveau compagnon
                var newCompanion = new Dictionary<string, List<string>>
                {
                    {
                        "AnimationIdle",
                        new List<string>
                        {
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Idle0.png",
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Idle1.png"
                        }
                    },
                    {
                        "AnimationSleep",
                        new List<string>
                        {
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Sleep0.png",
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Sleep1.png"
                        }
                    },
                    {
                        "AnimationWalk",
                        new List<string>
                        {
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Walk0.png",
                            "C:\\Users\\Asumi\\Desktop\\Projet Stusoft\\Visual_Studio\\Spiritecho777\\Echo\\Screenmate\\image\\Animation\\Renard\\renards_Walk1.png"
                        }
                    }
                };

                // Chargez la structure de dossier existante
                folderStructure = LoadFolderStructure();

                // Ajoutez le nouveau compagnon à la structure existante
                folderStructure.Folders[FolderName.Text] = newCompanion;

                // Ajoutez le chemin d'image à ImagePaths
                foreach (var image in newCompanion.SelectMany(pair => pair.Value))
                {
                    folderStructure.ImagePaths[image] = image;
                }

                // Mettez à jour le TreeView avec la nouvelle structure de dossier
                PopulateTreeView(folderStructure);

                // Sauvegardez la nouvelle structure de dossier dans le fichier
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");
                FileManager fileManager = new FileManager();
                fileManager.SaveFolderStructure(folderStructure, filePath);
            }
            else
            {
                FolderName.Text = "";
                MessageBox.Show("Veuillez donner un Nom au compagnon avant d'ajouter.");
                PopulateTreeView(folderStructure);
            }
        }

        private void PopulateTreeView(FolderStructure folderStructure)
        {
            SMFile.Items.Clear();   
            // Vérifiez si la structure de dossier est nulle
            if (folderStructure == null || folderStructure.Folders == null)
            {
                return;
            }

            // Parcourez les dossiers et ajoutez-les au TreeView
            foreach (var mainFolder in folderStructure.Folders)
            {
                TreeViewItem mainFolderItem = new TreeViewItem();
                mainFolderItem.Header = mainFolder.Key;

                // Parcourez les sous-dossiers et ajoutez-les au dossier principal
                foreach (var subFolder in mainFolder.Value)
                {
                    TreeViewItem subFolderItem = new TreeViewItem();
                    subFolderItem.Header = subFolder.Key;

                    // Parcourez les images et ajoutez-les au sous-dossier
                    foreach (var image in subFolder.Value)
                    {
                        TreeViewItem imageItem = new TreeViewItem();
                        imageItem.Header = image;
                        subFolderItem.Items.Add(imageItem);
                    }

                    // Ajoutez le sous-dossier au dossier principal
                    mainFolderItem.Items.Add(subFolderItem);
                }

                // Ajoutez le dossier principal au TreeView
                SMFile.Items.Add(mainFolderItem);
            }
        }

        private void ChangeSMPreview(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (folderStructure != null && folderStructure.ImagePaths != null && SMFile.SelectedItem != null && SMFile.SelectedItem is TreeViewItem selectedItem && selectedItem.Header is string imageName)
            {
                try
                {
                    // Récupérez le chemin complet du fichier image à partir de ImagePaths
                    if (folderStructure.ImagePaths.TryGetValue(imageName, out string imagePath))
                    {
                        // Chargez l'image et affichez-la dans PreviewImage
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.EndInit();
                        PreviewImage.Source = bitmap;
                    }
                    else
                    {
                        BitmapImage bitmaperr = new BitmapImage();
                        bitmaperr.BeginInit();
                        bitmaperr.UriSource = new Uri("image/MissedFile.png", UriKind.Relative);
                        bitmaperr.EndInit();
                        PreviewImage.Source = bitmaperr;
                    }
                }
                catch
                {
                    BitmapImage bitmaperr = new BitmapImage();
                    bitmaperr.BeginInit();
                    bitmaperr.UriSource = new Uri("image/MissedFile.png", UriKind.Relative); 
                    bitmaperr.EndInit();
                    PreviewImage.Source = bitmaperr;
                }
            }
        }
    }
}
