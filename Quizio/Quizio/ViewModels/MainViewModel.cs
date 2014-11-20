using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public RegularGameViewModel RegularGameViewModel { get; set; }

        public ICommand ReloadRankings { get; set; }

        public MainViewModel(User user, List<Category> categories,  List<Notification> notifications, List<Ranking> rankings)
        {
            this.CurrentUser = user;
            this.Categories = categories;
            this.Notifications = notifications;
            this.Rankings = rankings;
            this.RegularGameViewModel = new RegularGameViewModel(user, categories);

            this.ReloadRankings = new DelegateCommand(reloadRankings);
        }

        private void reloadRankings()
        {   
            RankingDAO rankingDao = new RankingDAO();
            Rankings = rankingDao.loadRankings();
        }
    }
}
