using Quizio.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class MultiplayerGameAggregator : GameAggregator
    {
        private string challengeText;
        private Category cat;

        #region DAO Interfaces
        private IMultiplayerGameDAO multiplayerGameDAO;
        #endregion

        public MultiplayerGameAggregator() : base()
        {
            this.multiplayerGameDAO = new MultiplayerGameDAO();
        }

        public MultiplayerGameAggregator(IMultiplayerGameDAO multiplayerGameDAO) : base()
        {
            this.multiplayerGameDAO = multiplayerGameDAO;
        }

        public void saveChallengeGame()
        {
            Game challangeGame = new Game(base.User, base.TimeNeededSum, base.CorrectUserInputs, base.Quiz, this.cat);
            multiplayerGameDAO.saveChallengeGame(challangeGame, challengeText);
        }
    }
}
