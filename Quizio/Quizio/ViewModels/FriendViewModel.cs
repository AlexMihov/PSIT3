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
    public class FriendViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        private bool _showOrHide;
        public bool ShowOrHide
        {
            get { return this._showOrHide; }
            set { SetProperty(ref this._showOrHide, value); }
        }

        public ICommand DeleteFriend { get; set; }
        public ICommand SearchFriends { get; set; }
        public ICommand AddFriend { get; set; }

        private BackgroundWorker bwDelete;
        private BackgroundWorker bwSearch;
        private BackgroundWorker bwAdd;

        public FriendViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;

            bwDelete = new BackgroundWorker();
            bwDelete.DoWork += new DoWorkEventHandler(bwDelete_DoWork);
            bwDelete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            bwAdd = new BackgroundWorker();
            bwAdd.DoWork += new DoWorkEventHandler(bwAdd_DoWork);
            bwAdd.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            this.DeleteFriend = new DelegateCommand(deleteFriend);
            this.SearchFriends = new DelegateCommand(searchFriends);
            this.AddFriend = new DelegateCommand(addFriend);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowOrHide = false;
            if (!(e.Result == null) && e.Cancelled)
            {
                ModernDialog.ShowMessage(e.Result as string, "Error", MessageBoxButton.OK);
            }
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    Aggregator.deleteFriend();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as exeption messanger
                e.Cancel = true;
            }
        }

        private void bwAdd_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    Aggregator.addFriend();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as exeption messanger
                e.Cancel = true;
            }
        }

        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (!worker.CancellationPending)
                {
                    Aggregator.searchFriends();
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as exeption messanger
                e.Cancel = true;
            }
        }

        private void deleteFriend()
        {
            ShowOrHide = true;
            if (!bwDelete.IsBusy)
            {
                bwDelete.RunWorkerAsync();
            }
        }

        private void searchFriends()
        {
            ShowOrHide = true;
            if (!bwSearch.IsBusy)
            {
                bwSearch.RunWorkerAsync();
            }
        }

        private void addFriend()
        {
            ShowOrHide = true;
            if (!bwAdd.IsBusy)
            {
                bwAdd.RunWorkerAsync();
            }
        }
    }
}
