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
    public class RankingViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;

        public RankingViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        }

        public void ReloadRankings()
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if(!worker.CancellationPending){
                    Aggregator.reloadRankings();
                }
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Verbindungsproblem!", MessageBoxButton.OK);
            }
        }
    }
}
