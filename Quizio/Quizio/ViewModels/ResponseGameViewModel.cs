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

        public ResponseGameViewModel(GameAggregator game) : base(game)
        {
        }

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
            ResponseGameAggregator rga = base.Game as ResponseGameAggregator;

            rga.sortRounds();

            base.CorrectUserInputs = rga.getCorrectRounds(rga.Rounds);
            base.FalseUserInputs = rga.getFalseRounds(rga.Rounds);
            base.TimedOutUserInputs = rga.getTimedOutRounds(rga.Rounds);

            CorrectChallengerInputs = rga.getCorrectRounds(rga.Challenge.ChallengeGame.Rounds);
            FalseChallengerInputs = rga.getFalseRounds(rga.Challenge.ChallengeGame.Rounds);
            TimedOutChallengerInputs = rga.getTimedOutRounds(rga.Challenge.ChallengeGame.Rounds);
        }

        internal override void finishGame()
        {
            ResponseGameAggregator rga = base.Game as ResponseGameAggregator;
            rga.saveChallengeResponse();
        }
    }
}
