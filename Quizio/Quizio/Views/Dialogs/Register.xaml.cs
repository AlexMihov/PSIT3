﻿using FirstFloor.ModernUI.Windows.Controls;
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

namespace Quizio.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : ModernDialog
    {
        private BackgroundWorker bw;

        public bool registered { get; private set; }

        public Register()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {this.CancelButton};

            this.bw = new BackgroundWorker();
            this.bw.WorkerSupportsCancellation = true;
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            this.CancelButton.Content = "Abbrechen";

            this.register_button.Click += Register_Click;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (!worker.CancellationPending)
            {
                try
                {
                    UserDAO userDao = new UserDAO();
                    userDao.registerUser(userName.Text, password.Password, status.Text, region.Text);
                }
                catch (InvalidOperationException) // special execption throw if username exists
                {
                    e.Cancel = true;
                }
                catch (Exception ex)
                {
                    e.Result = ex.Message; // abusing e.Result as error messaging
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled == true)
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Dieser Benutzername existiert bereits", "Hinweis", MessageBoxButton.OK);
            }
            else if (e.Result != null)
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage(e.Result as string, "Error", MessageBoxButton.OK);
            }
            else
            {
                this.registered = true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            loading.Visibility = System.Windows.Visibility.Visible;
            if (password.Password.Equals(rePassword.Password))
            {
                if (!userName.Text.Equals(""))
                {
                    if (!bw.IsBusy)
                    {
                        bw.RunWorkerAsync();
                    }
                }
                else
                {
                    this.loading.Visibility = System.Windows.Visibility.Hidden;
                    ModernDialog.ShowMessage("Der Benutzername ist für die Registration essentiell!", "Hinweis", MessageBoxButton.OK);
                }
            }
            else
            {
                this.loading.Visibility = System.Windows.Visibility.Hidden;
                ModernDialog.ShowMessage("Die Passwörter stimmen nicht überein!", "Hinweis", MessageBoxButton.OK);
            }
        }

    }
}
