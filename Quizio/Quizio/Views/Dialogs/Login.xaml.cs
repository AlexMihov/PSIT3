using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
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

namespace Quizio.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : ModernDialog
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool granted { get; private set; }

        public Login()
        {
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

            if (UserName == "admin" && Password == "admin")
            {
                granted = true;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                ModernDialog.ShowMessage("Invalid Username/Password combination!", "Error", MessageBoxButton.OK);
            }
            
        }
    }
}
