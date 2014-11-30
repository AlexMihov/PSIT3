using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ProfileViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
            this.UpdateUserSettings = new DelegateCommand<object>(updateUserSettings);
            this.ResetUserSettings = new DelegateCommand(resetUserSettings);

            this.toReset = Aggregator.User;
        }

        private void updateUserSettings(object parameter)
        {
            //Aggregator.updateUserSettings();
            //this.toReset = Aggregator.User;

            string pw = (string)parameter;

            if (!(pw.Equals("") == true))
            {
                changePassword(pw);
            }
            ModernDialog.ShowMessage(pw, "ich chumme ned in if", System.Windows.MessageBoxButton.OK);
        }

        private void changePassword(string pw)
        {
            //Aggregator.changePassword(pw);
            ModernDialog.ShowMessage(pw, "passwort", System.Windows.MessageBoxButton.OK);
        }

        private void resetUserSettings()
        {
            Aggregator.resetUserSettings(toReset);
        }

    }
}
