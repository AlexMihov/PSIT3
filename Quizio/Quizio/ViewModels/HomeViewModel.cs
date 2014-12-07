﻿using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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

namespace Quizio.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;
        private BackgroundWorker bw_startChallenge;
        private bool showAgain;
        private Challenge challengeToPlay;
        private GameAggregator gameAggregator;

        public ICommand RespondChallenge { get; set; }

        public HomeViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.showAgain = true;
            this.challengeToPlay = null;

            this.RespondChallenge = new DelegateCommand<object>(startChallengeGame);

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw_startChallenge = new BackgroundWorker();
            bw_startChallenge.DoWork += bw_startChallenge_DoWork;
            bw_startChallenge.RunWorkerCompleted += bw_startChallenge_RunWorkerCompleted;
        }

        void bw_startChallenge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ModernDialog.ShowMessage("Die Benutzerdaten konnten nicht gespeichert werden.", "Verbindungsfehler", System.Windows.MessageBoxButton.OK);
            }
            else
            {
                showChallengeWindow();
            }
            
        }

        void bw_startChallenge_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.gameAggregator = Aggregator.loadMultiplayerResponseGameData(challengeToPlay);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #region Commandfunctionallity
        private void startChallengeGame(object param)
        {
            challengeToPlay = param as Challenge;
            bw_startChallenge.RunWorkerAsync();
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
