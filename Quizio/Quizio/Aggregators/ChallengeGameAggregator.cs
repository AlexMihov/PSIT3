using Quizio.DAO;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Aggregators
{
    public class ChallengeGameAggregator : SoloGameAggregator
    {
        private string challengeText;
        private Player challengedFriend;

        #region DAO Interfaces
        internal IMultiplayerGameDAO multiplayerGameDAO;
        #endregion

        public ChallengeGameAggregator()
            : base()
        {
            this.multiplayerGameDAO = new MultiplayerGameDAO();
        }

        public ChallengeGameAggregator(Player challengedFriend, string challengeText)
            : base()
        {
            this.multiplayerGameDAO = new MultiplayerGameDAO();
            this.challengedFriend = challengedFriend;
            this.challengeText = challengeText;
        }

        public ChallengeGameAggregator(IMultiplayerGameDAO multiplayerGameDAO) : base()
        {
            this.multiplayerGameDAO = multiplayerGameDAO;
        }

        public void saveChallengeGame()
        {
            Game challangeGame = new Game(base.User, base.TimeNeededSum, base.Rounds, base.Quiz, null);
            multiplayerGameDAO.saveChallengeGame(challangeGame, challengedFriend, challengeText);
        }

        public void declineChallenge(Challenge challengeToDecline)
        {
            var challenge = challengeToDecline;
            challenge.ResponseGame = null;
            challenge.Status = "abgelehnt";
            multiplayerGameDAO.saveChallengeResponse(challenge);
        }
    }
}
