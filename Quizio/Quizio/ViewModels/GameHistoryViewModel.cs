using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Views.HomeViews.ResultView;
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
    public class GameHistoryViewModel : BindableBase
    {
        private bool _showOrHide;
        public bool ShowOrHide
        {
            get { return this._showOrHide; }
            set { SetProperty(ref this._showOrHide, value); }
        }

        public ModelAggregator Aggregator { get; set; }

        private Challenge _selectedChellange;
        public Challenge SelectedChallenge
        {
            get { return this._selectedChellange; }
            set { SetProperty(ref this._selectedChellange, value); }
        }

        public ICommand ShowChallengeResults { get; set; }

        private BackgroundWorker bw;
        private BackgroundWorker bw_show;
        private GameAggregator gameDataToShow;
        private bool showAgain;

        public GameHistoryViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.showAgain = true;

            this.ShowChallengeResults = new DelegateCommand(showChallengeResult);

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            this.ShowOrHide = false;

            bw_show = new BackgroundWorker();
            bw_show.DoWork += bw_show_DoWork;
            bw_show.RunWorkerCompleted += bw_show_RunWorkerCompleted;
        }

        void bw_show_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowOrHide = false;

            if (!(e.Result == null))
            {
                ModernDialog.ShowMessage(e.Result as string, "Fehler", MessageBoxButton.OK);
            }
            else
            {
                showResult();
            }
        }

        void bw_show_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    gameDataToShow = Aggregator.loadMultiplayerResponseGameData(SelectedChallenge);
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as exeption messanger
            }
        }

        private void showResult()
        {
            var wnd = new ResultWindow(new ResponseGameViewModel(gameDataToShow));
            wnd.Content = new MultiplayerGameResult();
            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wnd.Show();
        }

        private void showChallengeResult()
        {
            ShowOrHide = true;
            if (!bw_show.IsBusy)
            {
                bw_show.RunWorkerAsync();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Result != null) && showAgain)
            {
                MessageBoxResult result = ModernDialog.ShowMessage("Die Daten für die Spiel-Historie Ansicht konnten nicht aktuallisiert werden:\n" +
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
                    Aggregator.reloadAllChallenges();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        public void ReloadChallenges()
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
        }
    }
}
