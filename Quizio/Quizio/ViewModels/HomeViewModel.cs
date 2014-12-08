using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Aggregators;
using Quizio.Models;
using Quizio.Views.MultiplayerGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Quizio.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private bool _showOrHide;
        public bool ShowOrHide
        {
            get { return this._showOrHide; }
            set { SetProperty(ref this._showOrHide, value); }
        }

        private Visibility _showChallengeListEmpty;
        public Visibility ShowChallengeListEmpty
        {
            get { return this._showChallengeListEmpty; }
            set { SetProperty(ref this._showChallengeListEmpty, value); }
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

        private int timerTickCount;
        private DispatcherTimer myTimer;
        private static int COUNTDOWNTIME = 3;

        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;
        private BackgroundWorker bw_startChallenge;
        private BackgroundWorker bw_declineChallenge;
        private bool showAgain;
        private Challenge challengeToPlay;
        private SoloGameAggregator gameAggregator;

        public ICommand RespondChallenge { get; set; }
        public ICommand DeclineChallange { get; set; }

        public HomeViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.showAgain = true;
            this.challengeToPlay = null;

            this.RespondChallenge = new DelegateCommand<object>(startChallengeGame);
            this.DeclineChallange = new DelegateCommand<object>(declineChallenge);

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw_startChallenge = new BackgroundWorker();
            bw_startChallenge.DoWork += bw_startChallenge_DoWork;
            bw_startChallenge.RunWorkerCompleted += bw_startChallenge_RunWorkerCompleted;

            bw_declineChallenge = new BackgroundWorker();
            bw_declineChallenge.DoWork += bw_declineChallenge_DoWork;
            bw_declineChallenge.RunWorkerCompleted += bw_declineChallenge_RunWorkerCompleted;

            ShowOrHide = false;
            ShowChallengeListEmpty = Visibility.Collapsed;
            if (Aggregator.OpenChallenges.Count == 0)
            {
                ShowChallengeListEmpty = Visibility.Visible;
            }
            ShowCountDown = Visibility.Hidden;
            timerTickCount = 0;
            TimerTickCountDown = COUNTDOWNTIME;
            myTimer = new DispatcherTimer();
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += new EventHandler(Timer_Tick);
        }

        void bw_declineChallenge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Result == null))
            {
                ModernDialog.ShowMessage(e.Result as string, "Fehler", MessageBoxButton.OK);
            }
            else
            {
                ReloadHomeData();
                Aggregator.reloadAllChallenges();
            }
            this.ShowOrHide = false;
        }

        void bw_declineChallenge_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.gameAggregator = new ResponseGameAggregator(challengeToPlay);
                var rga = this.gameAggregator as ResponseGameAggregator;
                rga.declineChallenge(challengeToPlay);
                rga.updateRanking(challengeToPlay.ChallengeGame.Player, 100);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (++timerTickCount == COUNTDOWNTIME)
            {
                timer.Stop();
                showChallengeWindow();
                ShowCountDown = Visibility.Hidden;
                timerTickCount = 0;
            }
            TimerTickCountDown = COUNTDOWNTIME - timerTickCount;
        }

        void bw_startChallenge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowOrHide = false;

            if (!(e.Result == null))
            {
                ModernDialog.ShowMessage(e.Result as string, "Fehler", MessageBoxButton.OK);
            }
            else
            {
                myTimer.Start();
                ShowCountDown = Visibility.Visible;
            }
        }

        void bw_startChallenge_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.gameAggregator = Aggregator.loadResponseGameData(challengeToPlay);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        #region Commandfunctionallity
        private void startChallengeGame(object param)
        {
            challengeToPlay = param as Challenge;
            ShowOrHide = true;
            if (!bw_startChallenge.IsBusy)
            {
                bw_startChallenge.RunWorkerAsync();
            }
        }

        private void declineChallenge(object param)
        {
            challengeToPlay = param as Challenge;
            ShowOrHide = true;
            if (!bw_declineChallenge.IsBusy)
            {
                bw_declineChallenge.RunWorkerAsync();
            }
        }

        private void showChallengeWindow()
        {
            var rgvm = new ResponseGameViewModel(gameAggregator as ResponseGameAggregator);
            var wnd = new MultiplayerGameWindow(rgvm);
            App.Current.MainWindow.Hide(); //hide the mainwindow -> show after game ends or when user cancels the game

            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wnd.Show();
        }
        #endregion

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Result != null) && showAgain)
            {
                MessageBoxResult result =  ModernDialog.ShowMessage("Die Daten für die Homeansicht konnten nicht aktuallisiert werden:\n"+
                    e.Result as string + "\n\nWillst du weiterhin benachrichtigt werden?", "Verbingungsproblem!", System.Windows.MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    showAgain = false;
                }
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    Aggregator.reloadHomeData();
                    if (Aggregator.OpenChallenges.Count == 0)
                    {
                        ShowChallengeListEmpty = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        public void ReloadHomeData()
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
        }
    }
}
