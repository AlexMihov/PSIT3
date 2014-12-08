using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Aggregators;
using Quizio.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Quizio.ViewModels
{
    public class ProfileViewModel : BindableBase
    {
        #region ButtonCommands
        public ICommand UpdateUserSettings { get; set; }
        public ICommand ResetUserSettings { get; set; }
        #endregion

        private User toReset;

        public ModelAggregator Aggregator { get; set; }
        private BackgroundWorker bw;

        public ProfileViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.UpdateUserSettings = new DelegateCommand<object>(updateUserSettings);
            this.ResetUserSettings = new DelegateCommand(resetUserSettings);

            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            this.toReset = Aggregator.User;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ModernDialog.ShowMessage("Die Benutzerdaten konnten nicht gespeichert werden.", "Verbindungsfehler", System.Windows.MessageBoxButton.OK);
            }
            else
            {
                ModernDialog.ShowMessage("Die Daten wurden erfolgreich gespeichert.", "Erfolg", System.Windows.MessageBoxButton.OK);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Aggregator.updateUserSettings();
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void updateUserSettings(object parameter)
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
            //(this.toReset = Aggregator.User;
        }

        private void resetUserSettings()
        {
            Aggregator.resetUserSettings(toReset);
        }

    }
}
