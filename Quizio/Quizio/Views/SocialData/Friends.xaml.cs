using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace Quizio.Views
{
    /// <summary>
    /// Interaction logic for Friends.xaml
    /// </summary>
    public partial class Friends : UserControl
    {
        private BackgroundWorker bw;
        private List<Friend> SearchResult { get; set; }
        private string UserSearch { get; set; }

        public Friends()
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            InitializeComponent();
        }

        private void SEARCH_Click(object sender, RoutedEventArgs e)
        {
            UserSearch = searchUser.Text;

            //loading.Visibility = System.Windows.Visibility.Visible;

            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*if ((e.Cancelled == true))
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Invalid Username/Password combination!", "Error", MessageBoxButton.OK);
            }
            else if (!(e.Error == null))
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Error: " + e.Error.Message, "Error", MessageBoxButton.OK);
            }
            else
            {
                granted = true;
                this.DialogResult = true;
                this.Close();
            }*/
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
                SearchResult = userDao.searchFriends(UserSearch);
            }
        }

    }
}