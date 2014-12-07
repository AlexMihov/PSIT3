using Microsoft.Practices.Prism.Mvvm;
using Quizio.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<Challenge> _challenges;
        public List<Challenge> Challenges
        {
            get { return this._challenges; }
            set { SetProperty(ref this._challenges, value); }
        }

        private List<Ranking> _rankings;
        public  List<Ranking> Rankings
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

        #region DAO Interfaces
        private IHomeDAO homeDao;
        private IRankingDAO rankingDao;
        private ICategoryDAO catDao;
        private IUserDAO userDao;
        #endregion

        public ModelAggregator()
        {
            // create a new instance of swappable DAO and assign them to the private Interfaces
            homeDao = new HomeDAO();
            rankingDao = new RankingDAO();
            catDao = new CategoryDAO();
            userDao = new UserDAO();
        }

        public ModelAggregator(IHomeDAO homeDao, IRankingDAO rankingDao, ICategoryDAO catDao, IUserDAO userDao)
        {
            // assign swappable DAO to the private Interfaces
            this.homeDao = homeDao;
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

            loadCategories();
        }

        public GameAggregator loadGameData()
        {
            GameAggregator newGame = new GameAggregator();
            newGame.User = User;
            newGame.loadGameData(SelectedQuiz);
            return newGame;
        }

        public MultiplayerGameAggregator loadMultiplayerGameData(Player friendToChallange, string challangeText)
        {
            MultiplayerGameAggregator newGame = new MultiplayerGameAggregator(friendToChallange, challangeText);
            newGame.User = User;
            newGame.loadGameData(SelectedQuiz);
            return newGame;
        }

        public ResponseGameAggregator loadMultiplayerResponseGameData(Challenge challenge)
        {
            ResponseGameAggregator newGame = new ResponseGameAggregator(challenge.ChallengeGame);
            newGame.User = User;
            newGame.loadGameData(challenge.ChallengeGame.PlayedQuiz);
            return newGame;
        }

        #region related to HomeViewModel
        public void reloadHomeData()
        {
            Notifications = homeDao.loadNotifications();
            reloadChallenges();
        }
        public void reloadChallenges()
        {
            Challenges = homeDao.loadChallenges();
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

        #region related to RegularGameViewModel
        public void loadCategories()
        {
            Categories = catDao.loadCategories();
        }
        #endregion
    }
}
