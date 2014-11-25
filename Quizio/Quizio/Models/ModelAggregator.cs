using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class ModelAggregator
    {
        public List<Friend> Friends { get; set; }
        public User User { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Ranking> Rankings { get; set; }
        public List<Category> Categories { get; set; }

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

        public Game loadGameData(int id)
        {
            Game newGame = new Game();
            newGame.loadGameData(id);
            return newGame;
        }
    }
}
