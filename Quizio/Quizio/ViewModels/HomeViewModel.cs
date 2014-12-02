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
using System.Windows.Input;

namespace Quizio.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        private BackgroundWorker bw;

        public HomeViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Aggregator.loadData();
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Fehler mit der Verbingung!", System.Windows.MessageBoxButton.OK);
            }
        }

        public void ReloadHomeData()
        {
            bw.RunWorkerAsync();
        }
    }
}
