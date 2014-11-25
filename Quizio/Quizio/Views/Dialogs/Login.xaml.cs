using FirstFloor.ModernUI.Windows.Controls;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quizio.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : ModernDialog
    {
        private BackgroundWorker bw;

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool granted { get; private set; }

        //datafields for mainviewmodel constructor
        public List<Friend> Friends { get; private set; }
        public User User { get; set; }
        public List<Notification> Notifications { get; private set; }
        public List<Ranking> Rankings { get; private set; }
        public List<Category> Categories { get; private set; }
        //end datafields for mainviewmodel constructor

        public Login()
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            UserName = userName.Text;
            Password = password.Password;

            loading.Visibility = System.Windows.Visibility.Visible;

            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
            }
            else
            {
                UserDAO userDao = new UserDAO();
                try
                {
                    User = userDao.logIn(UserName, Password);

                    if (User != null)
                    {
                        User.loadFriends();

                        NotificationDAO natDAO = new NotificationDAO();
                        Notifications = natDAO.loadNotifications(User.Id);

                        RankingDAO rankingDAO = new RankingDAO();
                        Rankings = rankingDAO.loadRankings();

                        CategoryDAO catDao = new CategoryDAO();
                        Categories = catDao.loadCategories();
                    }
                    else
                    {
                        e.Cancel = true; // cancel the worker if user/pw not correct
                    }
                }
                catch (WebException ex)
                {
                    e.Result = ex.Message; // abusing e.Result as error messaging
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Invalid Username/Password combination!", "Error", MessageBoxButton.OK);
            }

            else if (e.Result != null)
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Error: " + e.Result, "Error", MessageBoxButton.OK);
            }

            else
            {
                granted = true;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
