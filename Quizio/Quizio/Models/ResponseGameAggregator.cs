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
            Game responseGame = new Game(base.User, base.TimeNeededSum, base.Rounds, base.Quiz, Challenge.ChallengeGame.Category);
            Challenge.ResponseGame = responseGame;
            Challenge.Status = "gespielt";
            base.multiplayerGameDAO.saveChallengeResponse(Challenge);
        }

        public void loadGameData(int challengeId)
        {
            Challenge = base.multiplayerGameDAO.getChallenge(challengeId);
        }

        public void sortRounds()
        {
            List<Round> challengerRounds = Challenge.ChallengeGame.Rounds;
            List<Round> responsePlayerRounds = base.Rounds;
            List<Round> sortedRounds = new List<Round>();
            var it = challengerRounds.GetEnumerator();
            
            while (it.MoveNext())
            {
                var challengerRound = it.Current;
                var it2 = responsePlayerRounds.GetEnumerator();
                while (it2.MoveNext())
                {
                    var responseRound = it2.Current;
                    if (responseRound.Question.Id == challengerRound.Question.Id)
                    {
                        sortedRounds.Add(responseRound);
                    }
                }
            }
            base.Rounds = sortedRounds;
        }

        public List<Round> getCorrectRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (round.isCorrect())
                {
                    result.Add(round);
                }
            }
            return result;
        }

        public List<Round> getFalseRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (!round.isCorrect() && !round.isTimedOut())
                {
                    result.Add(round);
                }
            }
            return result;
        }

        public List<Round> getTimedOutRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (round.isTimedOut())
                {
                    result.Add(round);
                }
            }
            return result;
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
