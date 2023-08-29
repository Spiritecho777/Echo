﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Windows.Media;

namespace Screenmate
{
    public partial class MainWindow : Window
    {
        #region Variables
        private SpeechRecognitionEngine speechEngine;
        private int currentFrameIndex = 0;
        private double targetVX = 0;
        private double targetVY = 0;
        private double timeElapsed = 0.0;
        private double frequency = 1.0;
        private double amplitude = 0.2;

        protected Point location;
        protected double vX, vY;

        public int height = 100;
        public int width = 67;

        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public static System.Windows.Threading.DispatcherTimer autonomyTimer = new System.Windows.Threading.DispatcherTimer();
        public static System.Windows.Threading.DispatcherTimer animationTimer = new System.Windows.Threading.DispatcherTimer();
        public static System.Windows.Threading.DispatcherTimer timerToSleep = new System.Windows.Threading.DispatcherTimer();

        Option Option = new Option();
        Procedure Procedure = new Procedure();
        Random alea = new Random();

        private List<string> animationIdle = new List<string>
        {
            "image/Animation/renards_Idle0.png",
            "image/Animation/renards_Idle1.png"
        };
        private List<string> animationSleep = new List<string>
        {
            "image/Animation/renards_sleep0.png",
            "image/Animation/renards_sleep1.png"
        };
        private List<string> animationMove = new List<string>
        {
            "image/Animation/renards_walk0.png",
            "image/Animation/renards_walk1.png"
        };

        private List<string> commandesVocales = new List<string>
        {
            "Programmation",
            "Machine Virtuel",
            "Procédures",
            "Deplace Toi", "Bouge",
            "Arrête", "Stop",
            "Donne moi la liste des commandes",
            "Lance les applications",
            "Extinction",
            "Ferme les applications"
        };
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            Mate.Height = height + 20;
            Mate.Width = width + 20;
            vX = 0;
            vY = 0;

            location = new Point((SystemParameters.PrimaryScreenWidth - Mate.ActualWidth) / 2, (SystemParameters.PrimaryScreenHeight - Mate.ActualHeight) / 2);

            #region timer
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += FoxMove;

            autonomyTimer.Interval = TimeSpan.FromSeconds(20);
            autonomyTimer.Tick += AutonomyTimer_Tick;

            timerToSleep.Interval = TimeSpan.FromSeconds(60);
            timerToSleep.Tick += TimerToSleep_Tick;

            animationTimer.Tick += FoxIdle;

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
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
            #endregion
        }

        #region Animation
        private void FoxIdle(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.50);
            img.Source = new BitmapImage(new Uri(animationIdle[currentFrameIndex], UriKind.Relative));
            currentFrameIndex = (currentFrameIndex + 1) % animationIdle.Count;
        }

        private void FoxSleep(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.75);
            img.Source = new BitmapImage(new Uri(animationSleep[currentFrameIndex], UriKind.Relative));
            currentFrameIndex = (currentFrameIndex + 1) % animationSleep.Count;
        }

        private void FoxMoveA(object sender, EventArgs e)
        {
            animationTimer.Interval = TimeSpan.FromSeconds(0.25);
            img.Source = new BitmapImage(new Uri(animationMove[currentFrameIndex], UriKind.Relative));
            currentFrameIndex = (currentFrameIndex + 1) % animationMove.Count;
        }

        private void FoxMove(object sender, EventArgs e)
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

            animationTimer.Tick -= FoxMoveA;
            animationTimer.Tick -= FoxSleep;
            animationTimer.Tick -= FoxIdle;
            animationTimer.Tick += FoxIdle;

            timerToSleep.Start();
        }

        private void TimerToSleep_Tick(object sender, EventArgs e)
        {
            animationTimer.Tick -= FoxSleep;
            animationTimer.Tick -= FoxMoveA;
            animationTimer.Tick -= FoxIdle;
            animationTimer.Tick += FoxSleep;
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

                animationTimer.Tick -= FoxMoveA;
                animationTimer.Tick -= FoxSleep;
                animationTimer.Tick -= FoxIdle;
                animationTimer.Tick += FoxMoveA;
            }
            else
            {
                timerToSleep.Stop();

                animationTimer.Tick -= FoxMoveA;
                animationTimer.Tick -= FoxSleep;
                animationTimer.Tick -= FoxIdle;
                animationTimer.Tick += FoxIdle;

                timerToSleep.Start();
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            timer.Stop();

            animationTimer.Tick -= FoxMoveA;
            animationTimer.Tick -= FoxSleep;
            animationTimer.Tick -= FoxIdle;
            animationTimer.Tick += FoxIdle;

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
        #endregion

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (Properties.Settings.Default.VocalEnabled)
            {
                string command = e.Result.Text;
                switch (command)
                {
                    case var s when s.Contains("Programmation"):
                        System.Diagnostics.Process.Start("C:/Program Files (x86)/Microsoft Visual Studio/2019/Community/Common7/IDE/devenv.exe");
                        //System.Diagnostics.Process.Start("C:/Program Files/Microsoft Visual Studio/2022/Community/Common7/IDE/devenv.exe");
                        break;
                    case var s when s.Contains("Machine Virtuel"):
                        System.Diagnostics.Process.Start("C:/Program Files (x86)/VMware/VMware Workstation/vmware.exe");
                        break;
                    case var s when s.Contains("Procédures"):
                        Procedure.Show();
                        break;
                    case var s when s.Contains("Deplace Toi") || s.Contains("Bouge"):
                        {
                            Option.allowedMove = true;

                            animationTimer.Tick -= FoxMoveA;
                            animationTimer.Tick -= FoxSleep;
                            animationTimer.Tick -= FoxIdle;
                            animationTimer.Tick += FoxMoveA;

                            timer.Start();
                            autonomyTimer.Start();
                            Option.MoveChangeColor();
                        }
                        break;
                    case var s when s.Contains("Arrête") || s.Contains("Stop"):
                        {
                            Option.allowedMove = false;
                            timer.Stop();
                            autonomyTimer.Stop();

                            animationTimer.Tick -= FoxMoveA;
                            animationTimer.Tick -= FoxSleep;
                            animationTimer.Tick -= FoxIdle;
                            animationTimer.Tick += FoxIdle;

                            Option.MoveChangeColor();
                        }
                        break;
                    case var s when s.Contains("Donne moi la liste des commandes"):
                        {
                            string listeCommandes = string.Join(Environment.NewLine, commandesVocales);
                            MessageBox.Show("Commandes vocales disponibles : " + Environment.NewLine + listeCommandes);
                        }
                        break;
                    case var s when s.Contains("Lance les applications"):
                        Option.StartDay();
                        break;
                    case var s when s.Contains("Ferme les applications"):
                        Option.EndDay();
                        break;
                    case var s when s.Contains("Extinction"):
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}