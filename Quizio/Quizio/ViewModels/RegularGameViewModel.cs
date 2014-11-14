using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
<<<<<<< HEAD
=======
<<<<<<< Updated upstream
using Quizio.Views;
=======
>>>>>>> Stashed changes
>>>>>>> 925372401c3fb132665a76daad52bb52bc22e7b1
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
<<<<<<< HEAD
        private Quiz selectedQuiz;

        public Quiz SelectedQuiz
        {
            get { return this.selectedQuiz; }
            set { SetProperty(ref this.selectedQuiz, value); }
        }

        public List<Category> Categories { get; set; }

        public RegularGameViewModel(List<Category> categories)
        {
            this.PlayCommand = new DelegateCommand(this.Play);
            this.Categories = categories;
        }

        public ICommand PlayCommand { get; private set; }

        private void Play()
        {
            if (selectedQuiz != null)
            {
                var wnd = new SoloGameWindow(new SoloGameViewModel(selectedQuiz));
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
=======
<<<<<<< Updated upstream
        public IEnumerable<Category> Categories { get; set; }

        public Quiz SelectedQuiz { get; set; }

        public RegularGameViewModel(List<Category> categories)
        {
            this.Categories = categories;
            this.PlayCommand = new DelegateCommand(Play);
        }

        public ICommand PlayCommand { get; set; }
        
        public void Play()
        {
            var soloVM = new SoloGameViewModel(SelectedQuiz);
            var wnd = new SoloGameWindow(soloVM);
        }
    }
}
=======
        private Quiz selectedQuiz;

        public Quiz SelectedQuiz
        {
            get { return this.selectedQuiz; }
            set { SetProperty(ref this.selectedQuiz, value); }
        }

        public List<Category> Categories { get; set; }

        public RegularGameViewModel(List<Category> categories)
        {
            this.PlayCommand = new DelegateCommand(this.Play);
            this.Categories = categories;
        }

        public ICommand PlayCommand { get; private set; }

        private void Play()
        {
            if (selectedQuiz != null)
            {
                var wnd = new SoloGameWindow(new SoloGameViewModel(selectedQuiz));
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
>>>>>>> Stashed changes
>>>>>>> 925372401c3fb132665a76daad52bb52bc22e7b1
