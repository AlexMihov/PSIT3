using FirstFloor.ModernUI.Presentation;
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
using System.Windows.Media;

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
            AppearanceManager.Current.AccentColor = Color.FromRgb(0xfa, 0x68, 0x00);
            var login = new Login();
            login.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (login.ShowDialog().Value && login.granted)
            {
                MainViewModel mvm = new MainViewModel(login.Aggregator);
                var mainWindow = new MainWindow(mvm);
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mainWindow.Show();
            }
            else
            {
                Current.Shutdown(-1);
            }
        }
    }
}