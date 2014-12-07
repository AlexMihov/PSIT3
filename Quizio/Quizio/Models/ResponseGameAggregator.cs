using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class ResponseGameAggregator : MultiplayerGameAggregator
    {
        private Challenge _challenge;
        public Challenge Challenge
        {
            get { return this._challenge; }
            set { SetProperty(ref this._challenge, value); }
        }

        public Player Challenger { get; set; }
        public int ChallangerTime { get; set; }

        public ResponseGameAggregator(Challenge challenge) : base()
        {
            loadGameData(challenge.Id);
            base.Quiz = challenge.ChallengeGame.PlayedQuiz;
            base.Quiz.Questions = filterQuestions(Challenge.ChallengeGame.Rounds);
            Challenger = Challenge.ChallengeGame.Player;
            ChallangerTime = Challenge.ChallengeGame.Time;
        }

        public void saveChallengeResponse()
        {
            base.multiplayerGameDAO.saveChallengeResponse(Challenge);
        }

        public void loadGameData(int challengeId)
        {
            Challenge = base.multiplayerGameDAO.getChallenge(challengeId);
        }

        private List<Question> filterQuestions(List<Round> rounds)
        {
            List<Question> questions = new List<Question>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                questions.Add(it.Current.Question);
            }
            return questions;
        }
    }
}
