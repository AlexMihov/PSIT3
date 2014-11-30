using Quizio.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace Quizio.Views
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
            save.Click += handlePasswordChange;
        }

        private void handlePasswordChange(object sender, RoutedEventArgs e){
            if (!password.Password.Equals(""))
            {
                UserDAO userDao = new UserDAO();
                userDao.changePassword(password.Password);
            }
        }
    }
}
