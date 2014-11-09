using FirstFloor.ModernUI.Windows.Controls;
using Quizio.Models;
using Quizio.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizio.ViewModels
{
    class RegularGameViewModel
    {
        public RegularGame RegularGame { get; set; }

        public RegularGameViewModel()
        {
            _canExecute = true;
            this.RegularGame = new RegularGame();
            this.Categories = new[] {"Mathematik", "Astronomie", "Sprachen", "Literatur", "Tiere", "Seismologie", "Biologie", "Physik" };
            this.Difficulties = new[] { "Einfach", "Mittel", "Schwer", "Godlike" };
        }

        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Difficulties { get; set; }

        private ICommand _clickCommand;
        public ICommand PlayCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => Play(), _canExecute));
            }
        }
        private bool _canExecute;
        public void Play()
        {
            Debug.WriteLine(RegularGame.SelectedCategory, RegularGame.Difficulty);
            var wnd = new ModernWindow
            {
                Style = (Style)App.Current.Resources["EmptyWindow"],
                Content = new SoloGame(RegularGame.SelectedCategory, RegularGame.Difficulty),
                Title = "Solo Game",
                Height = 600,
                Width= 1200
            };
            wnd.Show();
            this._canExecute = false;
        }
    }
}

public class CommandHandler : ICommand
{
    private Action _action;
    private bool _canExecute;
    public CommandHandler(Action action, bool canExecute)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        _action();
    }
}
