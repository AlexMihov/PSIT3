using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Views.SoloGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizio.ViewModels
{
    public class RegularGameViewModel : BindableBase
    {
        private Quiz selectedQuiz;
        private User user;

        public Quiz SelectedQuiz
        {
            get { return this.selectedQuiz; }
            set { SetProperty(ref this.selectedQuiz, value); }
        }

        public List<Category> Categories { get; set; }

        public RegularGameViewModel(User user, List<Category> categories)
        {
            this.user = user;
            this.PlayCommand = new DelegateCommand(this.Play);
            this.Categories = categories;
        }

        public ICommand PlayCommand { get; private set; }

        private void Play()
        {
            if (selectedQuiz != null)
            {
                var wnd = new SoloGameWindow(new SoloGameViewModel(user, selectedQuiz));
                App.Current.MainWindow.Hide(); //hide the mainwindow -> show after game ends or when user cancels the game
                wnd.Show();
            }
            else
            {
                ModernDialog.ShowMessage("Bitte wähle ein Quiz aus", "ERROR", MessageBoxButton.OK);
            }
        }

    }
}