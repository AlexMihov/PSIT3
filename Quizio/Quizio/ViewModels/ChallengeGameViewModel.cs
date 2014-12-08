using Quizio.Aggregators;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class ChallengeGameViewModel : SoloGameViewModel
    {
        public ChallengeGameViewModel(ChallengeGameAggregator gameAggregator)
            : base(gameAggregator){}

        internal override void finishGame()
        {
            ChallengeGameAggregator mga = base.GameAggregator as ChallengeGameAggregator;
            mga.saveChallengeGame();
        }
    }
}
