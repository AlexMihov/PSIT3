using Quizio.Models;
using Quizio.Views.SoloGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quizio.ViewModels
{
    public class PvpGameViewModel : RegularGameViewModel
    {
        private Player _friendToChallange;
        public Player FriendToChallange
        {
            get { return this._friendToChallange; }
            set { SetProperty(ref this._friendToChallange, value); }
        }

        private string _challangeText;
        public string ChallangeText
        {
            get { return this._challangeText; }
            set { SetProperty(ref this._challangeText, value); }
        }

        public PvpGameViewModel(ModelAggregator aggregator) : base(aggregator)
        {
        }

        internal override void loadGame()
        {
            base.gameToStart = base.Aggregator.loadMultiplayerGameData(FriendToChallange, ChallangeText);
        }

        internal override void createGameWindow()
        {
            var cgvm = new ChallangeGameViewModel(base.gameToStart as MultiplayerGameAggregator);
            var wnd = new SoloGameWindow(cgvm);
            App.Current.MainWindow.Hide(); //hide the mainwindow -> show after game ends or when user cancels the game
            base.ShowCountDown = Visibility.Hidden;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wnd.Show();
        }
    }
}
