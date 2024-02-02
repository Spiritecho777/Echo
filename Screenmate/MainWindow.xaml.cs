using Screenmate.Module;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Diagnostics;
using System.Speech.Synthesis;
using Screenmate.Classe;
using System.Linq;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Controls.Primitives;

namespace Screenmate
{
    public partial class MainWindow : Window
    {
        #region Variables
        private readonly SpeechRecognitionEngine speechEngine;
        private int currentFrameIndex = 0;
        private readonly double targetVX = 0;
        private readonly double targetVY = 0;
        private double timeElapsed = 0.0;
        private readonly double frequency = 1.0;
        private readonly double amplitude = 0.2;

        protected System.Windows.Point location;
        protected double vX, vY;

        public int height = 100;
        public int width = 67;

        public static DispatcherTimer timer = new DispatcherTimer();
        public static DispatcherTimer autonomyTimer = new DispatcherTimer();
        public static DispatcherTimer animationTimer = new DispatcherTimer();
        public static DispatcherTimer timerToSleep = new DispatcherTimer();

        private readonly DispatcherTimer popupTimer = new DispatcherTimer();

        readonly Option Option = new Option();
        readonly Random alea = new Random();
        readonly ChangeSM ChangeSM = new ChangeSM();

        public static List<string> animationIdle;
        public static List<string> animationSleep;
        public static List<string> animationMove;

        private readonly List<EventSave> EventDate = new List<EventSave>();
        private readonly List<String> EventDateString = new List<string>();
        private readonly List<String> EventContent = new List<string>();

        private readonly List<string> commandesVocales = new List<string>();
        readonly List<CmdVoc> cmdVoc = new List<CmdVoc>();
        readonly List<string> list0 = new List<string>();
        readonly List<string> list1 = new List<string>();
        readonly List<string> list2 = new List<string>();
        readonly List<string> list3 = new List<string>();
        readonly List<string> list4 = new List<string>();
        readonly List<string> list5 = new List<string>();
        readonly List<string> list6 = new List<string>();
        readonly List<string> list7 = new List<string>();
        readonly List<string> list8 = new List<string>();
        readonly List<string> list9 = new List<string>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            #region Commande vocal Load
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath = Path.Combine(appDirectory, "Cmdvoc.dat");

            if (File.Exists(filePath))
            {
                cmdVoc = CmdVoc.LoadCmdVoc(filePath);
                foreach (CmdVoc cmd in cmdVoc)
                {
                    commandesVocales.AddRange(cmd.Phrase);
                }               
            }
            #endregion
            
            #region EventCalendar Load    
            string appDirectory2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EchoData");
            string filePath2 = Path.Combine(appDirectory2, "Calendar.dat");

            if (!Directory.Exists(appDirectory2))
            {
                Directory.CreateDirectory(appDirectory2);
            }

            if (!File.Exists(filePath))
            {
                EventSave EventSave = new EventSave();
                EventSave.SaveEventSave(EventDate, filePath2);
                EventDate = EventSave.LoadEventSave(filePath2);
            }
            else
            {
                EventDate = EventSave.LoadEventSave(filePath2);
            }

            if (EventDate != null)
            {
                foreach (var EventSave in EventDate)
                {
                    EventDateString.Add(EventSave.Date);
                    EventContent.Add(EventSave.Content);
                }
            }
            #endregion

            #region Déplacement
            animationIdle = new List<string> (ChangeSM.animIdle);           
            animationSleep = new List<string> (ChangeSM.animSleep);
            animationMove = new List<string> (ChangeSM.animMove);

            Mate.Height = height + 20;
            Mate.Width = width + 20;
            vX = 0;
            vY = 0;

            location = new System.Windows.Point((SystemParameters.PrimaryScreenWidth - Mate.ActualWidth) / 2, (SystemParameters.PrimaryScreenHeight - Mate.ActualHeight) / 2);
            #endregion

            #region timer
            popupTimer = new DispatcherTimer();
            popupTimer.Interval = TimeSpan.FromMinutes(1);
            popupTimer.Tick += PopupTimer_Tick;
            popupTimer.Start();

            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += MateMove;

            autonomyTimer.Interval = TimeSpan.FromSeconds(20);
            autonomyTimer.Tick += AutonomyTimer_Tick;

            timerToSleep.Interval = TimeSpan.FromSeconds(60);
            timerToSleep.Tick += TimerToSleep_Tick;

            animationTimer.Tick += MateIdle;

            animationTimer.Start();
            timerToSleep.Start();
            #endregion

            #region Reconnaissance Voacale
            try
            {
                speechEngine = new SpeechRecognitionEngine();
                Choices commands = new Choices();
                for (int i = 0; i < commandesVocales.Count; i++)
                {
                    commands.Add(new string[] { commandesVocales[i] });
                }
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(commands);
                Grammar grammar = new Grammar(grammarBuilder);
                speechEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
                speechEngine.LoadGrammar(grammar);
                speechEngine.SetInputToDefaultAudioDevice();
                try
                {
                    speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Impossible de démarrer la reconnaissance vocale : " + ex.Message);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }

            #region setup list
            CmdVoc selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Déplacement du Compagnon");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list0.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Arrêt du Compagnon");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list1.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Liste des commandes");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list2.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Lancements des logiciels");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list3.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Fermetures des logiciels");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list4.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Explorateurs de fichiers");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list5.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Changement de compagnon");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list6.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Edition de commandes vocales");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list7.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Paramétrage des chemins d'accèes");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list8.Add(phrase);
                }
            }

            selectedCommand = cmdVoc.FirstOrDefault(cmd => cmd.Nom == "Quitter l'application");
            if (selectedCommand != null)
            {
                List<string> phrases = selectedCommand.Phrase;
                foreach (var phrase in phrases)
                {
                    list9.Add(phrase);
                }
            }
            #endregion
            #endregion
        }

        #region Animation
        private void MateIdle(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.50);
            img.Source = new BitmapImage(new Uri(animationIdle[currentFrameIndex], UriKind.Absolute));
            currentFrameIndex = (currentFrameIndex + 1) % animationIdle.Count;
        }

        private void MateSleep(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.75);
            img.Source = new BitmapImage(new Uri(animationSleep[currentFrameIndex], UriKind.Absolute));
            currentFrameIndex = (currentFrameIndex + 1) % animationSleep.Count;
        }

        private void MateMoveA(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.25);
            img.Source = new BitmapImage(new Uri(animationMove[currentFrameIndex], UriKind.Absolute));
            currentFrameIndex = (currentFrameIndex + 1) % animationMove.Count;
        }

        private void MateMove(object sender, EventArgs e)
        {
            timeElapsed += 0.01;
            double smoothFactor = 0.005;
            double deltaX = Lerp(vX, targetVX, smoothFactor) + amplitude * Math.Cos(frequency * timeElapsed);
            double deltaY = Lerp(vY, targetVY, smoothFactor) + amplitude * Math.Sin(frequency * timeElapsed);
            vX = deltaX;
            vY = deltaY;

            img.LayoutTransform = new ScaleTransform(Math.Sign(vX), 1);


            double accelerationX = RandomNumber(-0.1, 0.1);
            double accelerationY = RandomNumber(-0.1, 0.1);

            vX += accelerationX;
            vY += accelerationY;

            double maxSpeed = 0.5;
            if (vX > maxSpeed) vX = maxSpeed;
            if (vX < -maxSpeed) vX = -maxSpeed;
            if (vY > maxSpeed) vY = maxSpeed;
            if (vY < -maxSpeed) vY = -maxSpeed;

            location.X += vX;
            location.Y += vY;

            ClampLocationToScreen();

            this.Left = location.X;
            this.Top = location.Y;

            UpdateLayout();
        }
        #endregion

        #region Timer
        private void AutonomyTimer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            autonomyTimer.Stop();

            animationTimer.Tick -= MateMoveA;
            animationTimer.Tick -= MateSleep;
            animationTimer.Tick -= MateIdle;
            animationTimer.Tick += MateIdle;

            timerToSleep.Start();
        }

        private void TimerToSleep_Tick(object sender, EventArgs e)
        {
            animationTimer.Tick -= MateSleep;
            animationTimer.Tick -= MateMoveA;
            animationTimer.Tick -= MateIdle;
            animationTimer.Tick += MateSleep;
        }

        private void PopupTimer_Tick(object sender, EventArgs e)
        {
            CheckAndShowPopup();
        }
        #endregion

        #region Action_de_la_souris
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            timer.Stop();
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
            location.X = this.Left;
            location.Y = this.Top;
            if (Option.allowedMove == true)
            {
                timerToSleep.Stop();
                timer.Start();
                autonomyTimer.Start();

                animationTimer.Tick -= MateMoveA;
                animationTimer.Tick -= MateSleep;
                animationTimer.Tick -= MateIdle;
                animationTimer.Tick += MateMoveA;
            }
            else
            {
                timerToSleep.Stop();

                animationTimer.Tick -= MateMoveA;
                animationTimer.Tick -= MateSleep;
                animationTimer.Tick -= MateIdle;
                animationTimer.Tick += MateIdle;

                timerToSleep.Start();
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            timer.Stop();

            animationTimer.Tick -= MateMoveA;
            animationTimer.Tick -= MateSleep;
            animationTimer.Tick -= MateIdle;
            animationTimer.Tick += MateIdle;

            /*DateTime currentTime = DateTime.Now;
            string test = currentTime.ToString("dd MMMM", CultureInfo.CreateSpecificCulture("fr-FR"));
            MessageBox.Show(test + " - "+EventDateString[0]);*/
            Option.Show();
        }
        #endregion

        #region autre méthode
        private void ClampLocationToScreen()
        {
            double maxX = SystemParameters.PrimaryScreenWidth - Mate.ActualWidth;
            double maxY = SystemParameters.PrimaryScreenHeight - Mate.ActualHeight;

            location.X = location.X < 0 ? 0 : (location.X > maxX ? maxX : location.X);
            location.Y = location.Y < 0 ? 0 : (location.Y > maxY ? maxY : location.Y);
        }

        private double RandomNumber(double min, double max)
        {
            return alea.NextDouble() * (max - min) + min;
        }

        private double Lerp(double start, double end, double t)
        {
            return start + t * (end - start);
        }

        private void CheckAndShowPopup()
        {
            DateTime currentTime = DateTime.Now;
            DateTime Tomorrow = currentTime.AddDays(+1);

            string targetTime = Properties.Settings.Default.RappelT;
            string test = Tomorrow.ToString("d MMMM", CultureInfo.CreateSpecificCulture("fr-FR"));
            string test2 = currentTime.ToString("HH:mm");

            for (int i = 0; i < EventDateString.Count; i++)
            {
                if (string.Equals(test, EventDateString[i], StringComparison.OrdinalIgnoreCase) && test2 == targetTime)
                {
                    MessageBox.Show("Demain il y a: " + EventContent[i]);
                }
            }
        }
        #endregion

        #region Commande Vocal
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (Properties.Settings.Default.VocalEnabled)
            {
                string command = e.Result.Text;

                for (int i = 0; i < list0.Count;i++)
                {
                    if (command.Contains(list0[i]))
                    {
                        Option.allowedMove = true;

                        animationTimer.Tick -= MateMoveA;
                        animationTimer.Tick -= MateSleep;
                        animationTimer.Tick -= MateIdle;
                        animationTimer.Tick += MateMoveA;

                        timer.Start();
                        autonomyTimer.Start();
                        Option.MoveChangeColor();
                        break;
                    }
                }

                for (int i = 0; i < list1.Count;i++)
                {
                    if (command.Contains(list1[i]))
                    {
                        Option.allowedMove = false;
                        timer.Stop();
                        autonomyTimer.Stop();

                        animationTimer.Tick -= MateMoveA;
                        animationTimer.Tick -= MateSleep;
                        animationTimer.Tick -= MateIdle;
                        animationTimer.Tick += MateIdle;

                        Option.MoveChangeColor();
                        break;
                    }
                }

                for (int i = 0; i < list2.Count;i++)
                {
                    if (command.Contains(list2[i]))
                    {
                        string listeCommandes = string.Join(Environment.NewLine, commandesVocales);
                        MessageBox.Show("Commandes vocales disponibles : " + Environment.NewLine + listeCommandes);
                        break;
                    }
                }

                for(int i = 0;i <list3.Count;i++)
                {
                    if (command.Contains(list3[i]))
                    {
                        Option.StartDay();
                        break;
                    }
                }

                for(int i = 0; i < list4.Count; i++)
                {
                    if (command.Contains(list4[i]))
                    {
                        Option.EndDay();
                        break;
                    }
                }

                for(int i =0; i < list5.Count; i++)
                {
                    if (command.Contains(list5[i]))
                    {
                        Option.Explo();
                        break;
                    }
                }

                for(int i = 0; i < list6.Count; i++)
                {
                    if (command.Contains(list6[i]))
                    {
                        Option.ChangeC();
                        break;
                    }
                }

                for(int i = 0; i <list7.Count ; i++) 
                {
                    if (command.Contains(list7[i]))
                    {
                        Option.EditCmd();
                        break;
                    }
                }

                for(int i = 0;i<list8.Count ; i++)
                {
                    if (command.Contains(list8[i]))
                    {
                        Option.Param();
                        break;
                    }
                }

                for(int i = 0;i<list9.Count ; i++)
                {
                    if (command.Contains(list9[i]))
                    {
                        Option.Fermeture();
                        break;
                    }
                }
            }
        }
    }
    #endregion
}