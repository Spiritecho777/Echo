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

namespace Screenmate
{
    public partial class ChangeSM : Window
    {
        private FolderStructure folderStructure;
        public ChangeSM()
        {
            InitializeComponent();

            folderStructure = LoadFolderStructure();

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
            var NewMate = new FolderStructure
            {
                Folders = new Dictionary<string, Dictionary<string, List<string>>>
                {
                    {
                        "test",
                        new Dictionary<string, List<string>>
                        {
                            {
                                "AnimationIdle",
                                new List<string>
                                {
                                    "image/Animation/renards_Idle0.png",
                                    "image/Animation/renards_Idle1.png"
                                }
                            },

                            {
                                "AnimationSleep",
                                new List<string>
                                {
                                    "image/Animation/renards_Sleep0.png",
                                    "image/Animation/renards_Sleep1.png"
                                }
                            },

                            {
                                "AnimationWalk",
                                new List<string>
                                {
                                    "image/Animation/renards_Walk0.png",
                                    "image/Animation/renards_Walk1.png"
                                }
                            },
                        }
                    }
                }
            };

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");
            
            FileManager fileManager = new FileManager();
            fileManager.SaveFolderStructure(NewMate, filePath);
            FolderStructure folderStructure = LoadFolderStructure();
            PopulateTreeView(folderStructure);
        }

        private void PopulateTreeView(FolderStructure folderStructure)
        {
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
            if (SMFile.SelectedItem != null && SMFile.SelectedItem is TreeViewItem selectedItem && selectedItem.Header is string imageName)
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
                    // L'image n'existe pas dans ImagePaths, vous pouvez gérer cela ici (par exemple, afficher un message d'erreur dans PreviewImage)
                }
            }
        }

    }
}
