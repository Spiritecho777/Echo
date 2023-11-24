using System.Windows;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Screenmate.Module
{
    public partial class AddSpritesDialog : Window
    {
        public int NumberSpritesM = 0;
        public int NumberSpritesI = 0;
        public int NumberSpritesS = 0;

        public AddSpritesDialog()
        {
            InitializeComponent();

            NumberSpritesM = 0;
            NumberSpritesS = 0;
            NumberSpritesI = 0;
        }

        private void AddMoveClick (object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBMove.Items.Add(fileName);
                    NumberSpritesM++;
                }
            }
        }

        private void AddIdleClick(object sender, RoutedEventArgs e)
        {          
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBIdle.Items.Add(fileName);
                    NumberSpritesI++;
                }
            }
        }

        private void AddSleepClick(object sender, RoutedEventArgs e) 
        {            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBSleep.Items.Add(fileName);
                    NumberSpritesS++;
                }
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e) 
        {
            if (NumberSpritesM < 1 || NumberSpritesI < 1 || NumberSpritesS < 1) 
            {
                MessageBox.Show("Veuillez ajouter au moins une image dans chacun des champs");
            }
            else
            {
                if (NumberSpritesI!=NumberSpritesM || NumberSpritesI != NumberSpritesS || NumberSpritesM != NumberSpritesS)
                {
                    MessageBox.Show("Mettre le même nombre d'image dans chaque dossier");
                }
                else
                {
                    DialogResult = true;
                }              
            }
        }
    }
}
