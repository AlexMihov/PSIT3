using FirstFloor.ModernUI.Windows.Controls;
using Quizio.Aggregators;
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
    public class ChallengeGameSelectionViewModel : SoloGameSelectionViewModel
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

        public ChallengeGameSelectionViewModel(ModelAggregator aggregator) : base(aggregator)
        {
            ChallangeText = "";
        }

        internal override void loadGame()
        {
            base.gameAggregator = base.Aggregator.loadChallengeGameData(FriendToChallange, ChallangeText);
        }

        internal override void createGameWindow()
        {
            var cgvm = new ChallengeGameViewModel(base.gameAggregator as ChallengeGameAggregator);
            var wnd = new SoloGameWindow(cgvm);
            App.Current.MainWindow.Hide(); //hide the mainwindow -> show after game ends or when user cancels the game
            base.ShowCountDown = Visibility.Hidden;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wnd.Show();
            ChallangeText = "";
        }

        internal override void Play()
        {
            if (base.Aggregator.SelectedQuiz != null)
            {
                if (FriendToChallange != null)
                {
                    base.ShowOrHide = true;
                    if (!base.bw.IsBusy)
                    {
                        base.bw.RunWorkerAsync();
                    }
                }
                else
                {
                    ModernDialog.ShowMessage("Bitte wähle einen Freund für die Herausforderung aus", "Hinweis", MessageBoxButton.OK);
                }
            }
            else
            {
                ModernDialog.ShowMessage("Bitte wähle ein Quiz aus", "Hinweis", MessageBoxButton.OK);
            }
        }
    }
}
