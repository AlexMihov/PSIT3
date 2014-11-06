using Quizio.Pages.Dialogs;
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
                var mainWindow = new MainWindow(login.UserName);
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username/Password!", "Error", MessageBoxButton.OK);
                Current.Shutdown(-1);
            }
        }
    }
}
