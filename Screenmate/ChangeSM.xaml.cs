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

        public List<string> animIdle = new List<string>();
        public List<string> animSleep = new List<string>();
        public List<string> animMove = new List<string>();

        public ChangeSM()
        {
            InitializeComponent();

            string appDirectory2 = AppDomain.CurrentDomain.BaseDirectory;
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"EchoData");
            string filePath = Path.Combine(appDirectory, "Sprite.dat");
           
            if (!Directory.Exists(appDirectory))
            { 
                Directory.CreateDirectory(appDirectory);
            }
            
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
                                        Path.Combine(appDirectory2, "Default\\renards_Idle0.png"),
                                        Path.Combine(appDirectory2, "Default\\renards_Idle1.png")
                                    }
                                },
                                {
                                    "AnimationSleep",
                                    new List<string>
                                    {
                                        Path.Combine(appDirectory2, "Default\\renards_Sleep0.png"),
                                        Path.Combine(appDirectory2, "Default\\renards_Sleep1.png")
                                    }
                                },
                                {
                                    "AnimationWalk",
                                    new List<string>
                                    {
                                        Path.Combine(appDirectory2, "Default\\renards_Walk0.png"),
                                        Path.Combine(appDirectory2, "Default\\renards_Walk1.png")
                                    }
                                }
                            }
                        }
                    },
                        ImagePaths = new Dictionary<string, string>
                    {
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Idle0.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Idle0.png")
                        },
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Idle1.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Idle1.png")
                        },
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Sleep0.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Sleep0.png")
                        },
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Sleep1.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Sleep1.png")
                        },
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Walk0.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Walk0.png")
                        },
                        {
                            Path.Combine(appDirectory2, "Default\\renards_Walk1.png"),
                            Path.Combine(appDirectory2, "Default\\renards_Walk1.png")
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

            LoadSelectedCompanionAnimations(Properties.Settings.Default.SpriteSource);

            PopulateTreeView(folderStructure);
        }

        private FolderStructure LoadFolderStructure()
        {
            /*string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");*/

            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath = Path.Combine(appDirectory, "Sprite.dat");

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

        private void LoadSelectedCompanionAnimations(string selectedFolderName)
        {
            // Vérifiez si le dossier sélectionné existe dans la structure de dossier
            if (folderStructure.Folders.ContainsKey(selectedFolderName))
            {
                // Récupérez les listes d'animations correspondantes au dossier sélectionné
                var selectedFolder = folderStructure.Folders[selectedFolderName];

                // Vérifiez si les listes existent et sont non vides
                 if (selectedFolder.ContainsKey("AnimationIdle") && selectedFolder["AnimationIdle"].Count > 0)
                 {
                     animIdle = selectedFolder["AnimationIdle"];
                 }

                 if (selectedFolder.ContainsKey("AnimationSleep") && selectedFolder["AnimationSleep"].Count > 0)
                 {
                     animSleep = selectedFolder["AnimationSleep"];
                 }

                 if (selectedFolder.ContainsKey("AnimationWalk") && selectedFolder["AnimationWalk"].Count > 0)
                 {
                     animMove = selectedFolder["AnimationWalk"];
                 }
            }
            else
            {
                var selectedFolder = folderStructure.Folders["Renard (Défaut)"];
                Properties.Settings.Default.SpriteSource = "Renard (Défaut)";
                Properties.Settings.Default.Save();

                // Vérifiez si les listes existent et sont non vides
                if (selectedFolder.ContainsKey("AnimationIdle") && selectedFolder["AnimationIdle"].Count > 0)
                {
                    animIdle = selectedFolder["AnimationIdle"];
                }

                if (selectedFolder.ContainsKey("AnimationSleep") && selectedFolder["AnimationSleep"].Count > 0)
                {
                    animSleep = selectedFolder["AnimationSleep"];
                }

                if (selectedFolder.ContainsKey("AnimationWalk") && selectedFolder["AnimationWalk"].Count > 0)
                {
                    animMove = selectedFolder["AnimationWalk"];
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FolderName.Text != "Nom du compagnon a ajouter" && !string.IsNullOrWhiteSpace(FolderName.Text) && !folderStructure.Folders.ContainsKey(FolderName.Text))
            {
                AddSpritesDialog dialog = new AddSpritesDialog();
                if (dialog.ShowDialog() == true)
                {
                    // Créez le nouveau compagnon
                    var newCompanion = new Dictionary<string, List<string>>
                    {
                        {
                            "AnimationIdle",
                            new List<string>(dialog.LBIdle.Items.OfType<string>())
                        },
                        {
                            "AnimationSleep",
                            new List<string>(dialog.LBSleep.Items.OfType<string>())
                        },
                        {
                            "AnimationWalk",
                            new List<string>(dialog.LBMove.Items.OfType<string>())
                        }

                    };

                    // Chargez la structure de dossier existante
                    folderStructure = LoadFolderStructure();

                    // Ajoutez le nouveau compagnon à la structure existante
                    folderStructure.Folders[FolderName.Text] = newCompanion;

                    // Ajoutez le chemin d'image à ImagePaths
                    foreach (var image in newCompanion.SelectMany(pair => pair.Value))
                    {
                        //MessageBox.Show(image.ToString());
                        folderStructure.ImagePaths[image] = image;
                    }

                    // Mettez à jour le TreeView avec la nouvelle structure de dossier
                    PopulateTreeView(folderStructure);

                    // Sauvegardez la nouvelle structure de dossier dans le fichier
                    /*string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");*/

                    string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
                    string filePath = Path.Combine(appDirectory, "Sprite.dat");
                    FileManager fileManager = new FileManager();
                    fileManager.SaveFolderStructure(folderStructure, filePath);
                }
            }
            else
            {
                FolderName.Text = "";
                MessageBox.Show("Veuillez donner un Nom au compagnon avant d'ajouter.");
                PopulateTreeView(folderStructure);
            }
        }

        private void Del_Click(object sender, EventArgs e)
        {
            if (SMFile.SelectedItem != null && SMFile.SelectedItem is TreeViewItem selectedItem)
            {
                string folderName = selectedItem.Header.ToString();
                if (folderName != "Renard (Défaut)")
                {
                    SMFile.Items.Remove(SMFile.SelectedItem);
                    folderStructure.Folders.Remove(folderName);

                    /*string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = System.IO.Path.Combine(appDirectory, "Sprite.dat");*/

                    string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
                    string filePath = Path.Combine(appDirectory, "Sprite.dat");
                    FileManager fileManager = new FileManager();
                    fileManager.SaveFolderStructure(folderStructure, filePath);
                }
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
            else
            {

            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Sel_Click(object sender, RoutedEventArgs e)
        {
            if (SMFile.SelectedItem != null && SMFile.SelectedItem is TreeViewItem selectedItem)
            {
                string folderName = selectedItem.Header.ToString();
                Properties.Settings.Default.SpriteSource = folderName;
                Properties.Settings.Default.Save();
                LoadSelectedCompanionAnimations(Properties.Settings.Default.SpriteSource);
                MainWindow.animationIdle = animIdle;
                MainWindow.animationMove = animMove;
                MainWindow.animationSleep = animSleep;
            }
        }
    }
}
