using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Views.SoloGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Quizio.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class RegularGameViewModel : BindableBase
    {
        private bool _showOrHide;
        public bool ShowOrHide
        {
            get { return this._showOrHide; }
            set { SetProperty(ref this._showOrHide, value); }
        }

        private Visibility _showCountDown;
        public Visibility ShowCountDown
        {
            get { return this._showCountDown; }
            set { SetProperty(ref this._showCountDown, value); }
        }

        private int _timerTickCountDown;
        public int TimerTickCountDown
        {
            get { return this._timerTickCountDown; }
            set
            {
                SetProperty(ref this._timerTickCountDown, value);
            }
        }

        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;
        internal GameAggregator gameToStart;
        private int timerTickCount;
        private DispatcherTimer myTimer;
        private static int COUNTDOWNTIME = 3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregator"></param>
        public RegularGameViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;

            ShowOrHide = false;
            ShowCountDown = Visibility.Hidden;

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            timerTickCount = 0;
            TimerTickCountDown = COUNTDOWNTIME;
            myTimer = new DispatcherTimer();
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += new EventHandler(Timer_Tick);

            this.PlayCommand = new DelegateCommand(this.Play);
        }

        public ICommand PlayCommand { get; private set; }

        private void Play()
        {
            if (Aggregator.SelectedQuiz != null)
            {
                ShowOrHide = true;
                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }   
            }
            else
            {
                ModernDialog.ShowMessage("Bitte wähle ein Quiz aus", "Fehler", MessageBoxButton.OK);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    loadGame();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as exeption messanger
            }
        }

        internal virtual void loadGame()
        {
            gameToStart = Aggregator.loadGameData();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowOrHide = false;

            if (!(e.Result == null))
            {
                ModernDialog.ShowMessage(e.Result as string, "Error", MessageBoxButton.OK);
            }
            else
            {
                myTimer.Start();
                ShowCountDown = Visibility.Visible;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (++timerTickCount == COUNTDOWNTIME)
            {
                timer.Stop();
                createGameWindow();

                timerTickCount = 0;
            }
            TimerTickCountDown = COUNTDOWNTIME - timerTickCount;
        }

        internal virtual void createGameWindow(){
            var wnd = new SoloGameWindow(new SoloGameViewModel(gameToStart));
            App.Current.MainWindow.Hide(); //hide the mainwindow -> show after game ends or when user cancels the game
            ShowCountDown = Visibility.Hidden;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wnd.Show();
        }

    }
}
