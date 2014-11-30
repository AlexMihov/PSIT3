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

        private string userNameField;

        public bool granted { get; private set; }

        //datafields for mainviewmodel constructor
        public ModelAggregator Aggregator { get; private set; }
        //end datafields for mainviewmodel constructor

        public Login()
        {
            Aggregator = new ModelAggregator();

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            InitializeComponent();
            this.CloseButton.Content = "Schliessen";
            this.CloseButton.Click += Cancel_Click;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerDialog = new Register();
            registerDialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            if (registerDialog.ShowDialog().Value && registerDialog.registered)
            {
                userName.Text = registerDialog.userName.Text;
                this.Show();

                registerDialog.Close();
            }
            else
            {
                registerDialog.Close();
            }           
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            userNameField = userName.Text;

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
                try
                {
                    Aggregator.logIn(userNameField, password.Password);

                    if (Aggregator.User != null)
                    {
                        Aggregator.loadData();
                    }
                    else
                    {
                        e.Cancel = true; // cancel the worker if user/pw not correct
                    }
                }
                catch (Exception ex)
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
                ModernDialog.ShowMessage("Ungültige Benutzername/Passwort Kombination!", "Hinweis", MessageBoxButton.OK);
            }

            else if (e.Result != null)
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage(e.Result as string, "Error", MessageBoxButton.OK);
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