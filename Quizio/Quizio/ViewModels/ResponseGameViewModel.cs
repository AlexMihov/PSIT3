using Quizio.Aggregators;
using Quizio.Models;
using Quizio.Views.MultiplayerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class ResponseGameViewModel : SoloGameViewModel
    {
        public List<Round> CorrectChallengerInputs { get; set; }
        public List<Round> FalseChallengerInputs { get; set; }
        public List<Round> TimedOutChallengerInputs { get; set; }

        public int ChallengerPoints { get; set; }
        public int ResponsePlayerPoints { get; set; }

        public ResponseGameViewModel(ResponseGameAggregator game)
            : base(game){}

        internal override void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "Play":
                    base.ContentControlView = new MultiplayerGamePlay();
                    break;

                case "Result":
                    base.ContentControlView = new MultiplayerGameResult();
                    break;
            }
        }

        internal override void fillListsOfRounds()
        {
            ResponseGameAggregator rga = base.GameAggregator as ResponseGameAggregator;

            rga.sortRounds();

            base.CorrectUserInputs = rga.getCorrectRounds(rga.Rounds);
            base.FalseUserInputs = rga.getFalseRounds(rga.Rounds);
            base.TimedOutUserInputs = rga.getTimedOutRounds(rga.Rounds);

            CorrectChallengerInputs = rga.getCorrectRounds(rga.Challenge.ChallengeGame.Rounds);
            FalseChallengerInputs = rga.getFalseRounds(rga.Challenge.ChallengeGame.Rounds);
            TimedOutChallengerInputs = rga.getTimedOutRounds(rga.Challenge.ChallengeGame.Rounds);

            ResponsePlayerPoints = CorrectUserInputs.Count * ((QuestionsRemaining * ANSWERTIME) - GameAggregator.TimeNeededSum);
            ChallengerPoints = CorrectChallengerInputs.Count * ((QuestionsRemaining * ANSWERTIME) - rga.Challenge.ChallengeGame.Time);
        }

        internal override void finishGame()
        {
            ResponseGameAggregator rga = base.GameAggregator as ResponseGameAggregator;
            rga.saveChallengeResponse();

            if (ResponsePlayerPoints > ChallengerPoints)
            {
                rga.updateRanking(rga.User, ResponsePlayerPoints);
            }
            else if (ResponsePlayerPoints == ChallengerPoints)
            {
                rga.updateRanking(rga.Challenge.ChallengeGame.Player, (int)ChallengerPoints / 2);
                rga.updateRanking(rga.User, (int)ResponsePlayerPoints / 2);
            }
            else
            {
                rga.updateRanking(rga.Challenge.ChallengeGame.Player, ChallengerPoints);
            }
        }

        internal override void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ContentControlView.GetType().IsInstanceOfType(new MultiplayerGameResult()))
            {
                base.save();
            }
            App.Current.MainWindow.Show();
        }

        internal override void reloadHomeView()
        {
            MainViewModel mvm = App.Current.MainWindow.DataContext as MainViewModel;
            mvm.HomeViewModel.ReloadHomeData();
            mvm.HomeViewModel.Aggregator.reloadAllChallenges();
        }
    }
}
