using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
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
        public ModelAggregator Aggregator { get; set; }

        private Challenge _selectedChallange;
        public Challenge SelectedChallange
        {
            get { return this._selectedChallange; }
            set { SetProperty(ref this._selectedChallange, value); }
        }

        public ICommand ShowChallangeResults;

        private BackgroundWorker bw;
        private bool showAgain;

        public GameHistoryViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.showAgain = true;

            this.ShowChallangeResults = new DelegateCommand(showChallangeResult);

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        private void showChallangeResult()
        {
            //show it
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
                    Aggregator.reloadChallenges();
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
