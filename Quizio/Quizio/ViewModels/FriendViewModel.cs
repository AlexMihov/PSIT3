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

        private Visibility _showFriend;
        public Visibility ShowFriend
        {
            get { return this._showFriend; }
            set { SetProperty(ref this._showFriend, value); }
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
            if (Aggregator.SelectedFriend == null)
            {
                this.ShowFriend = Visibility.Hidden;
            }
            else
            {
                this.ShowFriend = Visibility.Visible;
            }
            
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
            if (Aggregator.SelectedFriend == null || Aggregator.Friends == null)
            {
                ShowFriend = Visibility.Hidden;
            }
            else
            {
                ShowFriend = Visibility.Visible;
            }

            if (!(e.Result == null))
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
                e.Result = ex.Message; // e.Result abused as exception messanger
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
                e.Result = ex.Message; // e.Result abused as exception messanger
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
                e.Result = ex.Message; // e.Result abused as exception messanger
            }
        }

        private void deleteFriend()
        {

            if (!bwDelete.IsBusy)
            {
                bwDelete.RunWorkerAsync();
            }

            List<Friend> f = Aggregator.Friends.ToList();
            f.Remove(Aggregator.SelectedFriend);
            Aggregator.Friends = f;

            Aggregator.SelectedFriend = f.FirstOrDefault();
            
        }

        private void searchFriends()
        {
            if (Aggregator.FriendSearch != null && Aggregator.FriendSearch != "")
            {
                if (!bwSearch.IsBusy)
                {
                    bwSearch.RunWorkerAsync();
                }
            }
            else
            {
                ModernDialog.ShowMessage("Bitte gib einen Namen in die Suche ein!", "Hinweis", MessageBoxButton.OK);
            }
            
        }

        private void addFriend()
        {

            if (Aggregator.SelectedResult != null)
            {
                if (!bwAdd.IsBusy)
                {
                    bwAdd.RunWorkerAsync();
                }

                List<Friend> f = Aggregator.Friends.ToList();
                f.Add(Aggregator.SelectedResult);
                Aggregator.Friends = f;

                List<Friend> s = Aggregator.SearchResult.ToList();
                s.Remove(Aggregator.SelectedResult);
                Aggregator.SearchResult = s;

                Aggregator.SelectedFriend = f.FirstOrDefault();
            }
            else
            {
                ModernDialog.ShowMessage("Bitte wähle eine Person aus!", "Hinweis", MessageBoxButton.OK);
            }
        }
    }
}
