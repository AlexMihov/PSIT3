using Quizio.Models;
using Quizio.Pages.Dialogs;
using Quizio.ViewModels;
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
                //data retrieve simulation
                string userName = login.userName.Text;

                User currentUser = new User(userName, "zh", "ich lieb quizio", new List<Friend>());

                List<Category> categories = new List<Category>();

                List<Notification> notifications = new List<Notification>();

                List<Ranking> rankings = new List<Ranking>();


                //data retrieve simulation end
                var mvm = new MainViewModel(currentUser, categories, notifications, rankings);
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
