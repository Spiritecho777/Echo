using System.Windows;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Screenmate
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
            NumberSpritesM++;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBMove.Items.Add(fileName);
                }
            }
        }

        private void AddIdleClick(object sender, RoutedEventArgs e)
        {
            NumberSpritesI++;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBIdle.Items.Add(fileName);
                }
            }
        }

        private void AddSleepClick(object sender, RoutedEventArgs e) 
        {
            NumberSpritesS++;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers image (*.png;*.jpg)|*.png;*.jpg|Tous les fichiers (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LBSleep.Items.Add(fileName);
                }
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e) 
        {
            if (NumberSpritesM < 1 || NumberSpritesI < 1 || NumberSpritesS < 1) 
            {
                System.Windows.MessageBox.Show("Veuillez ajouter au moins une image dans chacun des champs");
            }
            else
            {
                DialogResult = true;
                
            }
        }
    }
}
