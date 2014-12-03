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
        public virtual List<Ranking> Rankings
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

        private Friend _selectedFriend;
        public Friend SelectedFriend
        {
            get { return this._selectedFriend; }
            set { SetProperty(ref this._selectedFriend, value); }
        }

        private String _friendSearch;
        public String FriendSearch
        {
            get { return this._friendSearch; }
            set { SetProperty(ref this._friendSearch, value); }
        }

        private List<Friend> _searchResult;
        public List<Friend> SearchResult
        {
            get { return this._searchResult; }
            set { SetProperty(ref this._searchResult, value); }
        }

        private Friend _selectedResult;
        public Friend SelectedResult
        {
            get { return this._selectedResult; }
            set { SetProperty(ref this._selectedResult, value); }
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

        public ModelAggregator(NotificationDAO natDao, RankingDAO rankingDao, CategoryDAO catDao, UserDAO userDao)
        {
            this.natDao = natDao;
            this.rankingDao = rankingDao;
            this.catDao = catDao;
            this.userDao = userDao;
        }

        public void logIn(string userName, string password)
        {
            User = userDao.logIn(userName, password);
        }

        public void loadData()
        {
            loadFriends();

            reloadHomeData();

            loadRankings();

            Categories = catDao.loadCategories();
        }

        public GameAggregator loadGameData()
        {
            GameAggregator newGame = new GameAggregator();
            newGame.User = User;
            newGame.loadGameData(SelectedQuiz);
            return newGame;
        }

        #region related to HomeViewModel
        public void reloadHomeData()
        {
            Notifications = natDao.loadNotifications(User.Id);
        }
        #endregion

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
        public virtual void loadRankings()
        {
            Rankings = rankingDao.loadRankings();
        }
        #endregion

        #region related to FriendViewModel
        public void loadFriends()
        {
            Friends = userDao.loadFriends();
            SelectedFriend = Friends.FirstOrDefault();
        }
        
        public void addFriend()
        {
            userDao.addNewFriend(SelectedResult.Id);
        }

        public void deleteFriend()
        {
            userDao.deleteFriend(SelectedFriend.Id);
        }

        public void searchFriends()
        {
            SearchResult = userDao.searchFriends(FriendSearch);
        }
        #endregion

    }
}
