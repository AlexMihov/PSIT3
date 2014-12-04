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
    public class HomeViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;
        private bool showAgain;

        public HomeViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.showAgain = true;

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

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
