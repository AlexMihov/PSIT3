using Microsoft.Practices.Prism.Mvvm;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class ModelAggregator : BindableBase
    {
        #region aggregated Models
        private List<Friend> _friends;
        public List<Friend> Friends
        {
            get { return this._friends; }
            set { SetProperty(ref this._friends, value); }
        }

        private User _user;
        public User User
        {
            get { return this._user; }
            set { SetProperty(ref this._user, value); }
        }

        private List<Notification> _notifications;
        public List<Notification> Notifications
        {
            get { return this._notifications; }
            set { SetProperty(ref this._notifications, value); }
        }

        private List<Ranking> _rankings;
        public List<Ranking> Rankings
        {
            get { return this._rankings; }
            set { SetProperty(ref this._rankings, value); }
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get { return this._categories; }
            set { SetProperty(ref this._categories, value); }
        }

        private Quiz _selectedQuiz;
        public Quiz SelectedQuiz
        {
            get { return this._selectedQuiz; }
            set { SetProperty(ref this._selectedQuiz, value); }
        }
        #endregion

        #region DAO's
        private NotificationDAO natDao;
        private RankingDAO rankingDao;
        private CategoryDAO catDao;
        private UserDAO userDao;
        #endregion

        public ModelAggregator()
        {
            natDao = new NotificationDAO();
            rankingDao = new RankingDAO();
            catDao = new CategoryDAO();
            userDao = new UserDAO();
        }

        public void logIn(string userName, string password)
        {
            User = userDao.logIn(userName, password);
        }

        public void loadData()
        {
            User.loadFriends();

            Notifications = natDao.loadNotifications(User.Id);

            Rankings = rankingDao.loadRankings();

            Categories = catDao.loadCategories();
        }

        public void reloadRankings()
        {
            Rankings = rankingDao.loadRankings();
        }

        public Game loadGameData()
        {
            Game newGame = new Game();
            newGame.User = User;
            newGame.loadGameData(SelectedQuiz);
            return newGame;
        }

        #region related to ProfileViewModel
        public void updateUserSettings()
        {
            userDao.updateUserSettings(this.User);
        }

        public void resetUserSettings(User toReset)
        {
            this.User = toReset;
        }
        #endregion

        #region related to RankingViewModel
        public void loadRankings()
        {
            Rankings = rankingDao.loadRankings();
        }
        #endregion
    }
}
