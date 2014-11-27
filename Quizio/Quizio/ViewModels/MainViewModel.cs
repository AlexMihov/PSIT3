using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Quizio.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public User CurrentUser { get; private set; }

        public IEnumerable<Notification> Notifications { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        private IEnumerable<Ranking> _rankings;
        public IEnumerable<Ranking> Rankings
        {
            get { return this._rankings; }
            set { SetProperty(ref this._rankings, value); }
        }

        private bool _showOrHide;
        public bool ShowOrHide
        {
            get { return this._showOrHide; }
            set { SetProperty(ref this._showOrHide, value); }
        }

        public RegularGameViewModel RegularGameViewModel { get; set; }
        public ProfileViewModel ProfileViewModel { get; set; }

        public ICommand ReloadRankings { get; set; }

        private BackgroundWorker bw;

        public MainViewModel(ModelAggregator aggregator)
        {
            this.CurrentUser = aggregator.User;
            this.Categories = aggregator.Categories;
            this.Notifications = aggregator.Notifications;
            this.Rankings = aggregator.Rankings;
            this.RegularGameViewModel = new RegularGameViewModel(CurrentUser, Categories as List<Category>);
            this.ProfileViewModel = new ProfileViewModel(aggregator);

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            this.ReloadRankings = new DelegateCommand(reloadRankings);
            this.ShowOrHide = false;
        }

        private void reloadRankings()
        {
            ShowOrHide = true;
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            RankingDAO rankingDao = new RankingDAO();

            try
            {
                Rankings = rankingDao.loadRankings();
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // e.Result abused as Exeptionmessanger
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowOrHide = false;

            if (!(e.Result == null))
            {
                ModernDialog.ShowMessage(e.Result as string, "Error", MessageBoxButton.OK);
            }
            
        }
    }
}
