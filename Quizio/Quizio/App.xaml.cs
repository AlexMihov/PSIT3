using Quizio.Models;
using Quizio.Utilities;
using Quizio.ViewModels;
using Quizio.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quizio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = new Login();

            if (login.ShowDialog() == true && login.granted)
            {
                //data retrieve simulation (to be fetched from DB)
                string name = login.userName.Text;
                string currentStatus = "I love Quizio";
                string location = "Jerusalem";

                Friend hans = new Friend("Hans", "Muster bedumtsch");
                Friend fritz = new Friend("Fritz", "Ritz bedumtsch");
                Friend rudolf = new Friend("Rudolf", "Liebt Golf bedumtsch");
                Friend michel = new Friend("Michel", "Mit der Sichel bedumtsch");

                List<Friend> friends = new List<Friend>();
                friends.Add(hans);
                friends.Add(fritz);
                friends.Add(rudolf);
                friends.Add(michel);

                User user = new User(name, currentStatus, friends);
                user.Location = location;
                user.Id = 2;

                NotificationDAO natDAO = new NotificationDAO();
                List<Notification> notifications = natDAO.loadNotifications(user.Id);

                RankingDAO rankingDAO = new RankingDAO();
                List<Ranking> rankings = rankingDAO.loadRankings();

                CategoryDAO catDao = new CategoryDAO();
                List<Category> categories = catDao.loadCategories();

                MainViewModel mvm = new MainViewModel(user, categories, notifications, rankings);
                //data retrieve simulation end

                var mainWindow = new MainWindow(mvm);
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Current.Shutdown(-1);
            }
        }
    }
}
