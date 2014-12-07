using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class ChallangeGameViewModel : SoloGameViewModel
    {
        public ChallangeGameViewModel(MultiplayerGameAggregator gameAggregator)
            : base(gameAggregator)
        {
        }

        internal override void finishGame()
        {
            MultiplayerGameAggregator mga = base.Game as MultiplayerGameAggregator;
            mga.saveChallengeGame();
        }
    }
}
