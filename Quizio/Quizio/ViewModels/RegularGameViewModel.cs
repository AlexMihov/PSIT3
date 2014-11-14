using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using Quizio.Views;
using Quizio.Views.SoloGame;
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
    public class RegularGameViewModel : BindableBase
    {
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
