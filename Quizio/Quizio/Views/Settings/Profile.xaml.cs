using Quizio.DAO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Quizio.Views
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        private BackgroundWorker bw;
        public Profile()
        {
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            InitializeComponent();
            save.Click += handlePasswordChange;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                UserDAO userDao = new UserDAO();
                userDao.changePassword(password.Password);
            }
            catch (Exception){}
        }

        private void handlePasswordChange(object sender, RoutedEventArgs e){
            if (!password.Password.Equals(""))
            {
                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }
                
            }
        }
    }
}
