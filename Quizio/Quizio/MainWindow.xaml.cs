using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism;
using Quizio.Views.Dialogs;
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
using Quizio.ViewModels;
using Quizio.Utilities;

namespace Quizio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow, IView
    {
        public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;

            this.Closing += MainWindow_Closing;
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                UserDAO userDao = new UserDAO();
                userDao.logOut();

                App.Current.MainWindow.Hide();

                MessageBoxResult result = ModernDialog.ShowMessage("Du wurdest erfolgreich ausgeloggt, möchtest du dich mit einem anderen Benutzernamen einloggen?", "Hinweis", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //Disable shutdown when the dialog closes
                    App.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                    var login = new Login();
                    login.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    if (login.ShowDialog().Value && login.granted)
                    {
                        MainViewModel mvm = new MainViewModel(login.Aggregator);
                        var mainWindow = new MainWindow(mvm);
                        //Re-enable normal shutdown mode.
                        App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        App.Current.MainWindow = mainWindow;
                        mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        mainWindow.Show();
                    }
                    else
                    {
                        App.Current.Shutdown(-1);
                    }
                }
                else
                {
                    App.Current.Shutdown(-1);
                }
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage("Das Ausloggen hat nicht funktioniert:\n"+
                    ex.Message+
                    "\n\nDie Applikation wird beendet.",
                    "Fehler", MessageBoxButton.OK);
                App.Current.Shutdown(-1);
            }
        }
    }
}