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
        private Player challengedFriend;

        #region DAO Interfaces
        private IMultiplayerGameDAO multiplayerGameDAO;
        #endregion

        public MultiplayerGameAggregator()
            : base()
        {
            this.multiplayerGameDAO = new MultiplayerGameDAO();
        }

        public MultiplayerGameAggregator(Player challengedFriend, string challengeText)
            : base()
        {
            this.multiplayerGameDAO = new MultiplayerGameDAO();
            this.challengedFriend = challengedFriend;
            this.challengeText = challengeText;
        }

        public MultiplayerGameAggregator(IMultiplayerGameDAO multiplayerGameDAO) : base()
        {
            this.multiplayerGameDAO = multiplayerGameDAO;
        }

        public void saveChallengeGame()
        {
            Game challangeGame = new Game(base.User, base.TimeNeededSum, base.Rounds, base.Quiz, null);
            multiplayerGameDAO.saveChallengeGame(challangeGame, challengedFriend, challengeText);
        }
    }
}
