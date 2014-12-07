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

        private List<Question> fullQuizList;

        public ResponseGameAggregator(Challenge challenge) : base()
        {
            Challenge = challenge;
            base.Quiz = challenge.ChallengeGame.PlayedQuiz;
            loadGameData();
            base.Quiz.Questions = filterQuestions(challenge.ChallengeGame.Rounds);
            Challenger = challenge.ChallengeGame.Player;
            ChallangerTime = challenge.ChallengeGame.Time;
        }

        public void saveChallengeResponse()
        {
            base.multiplayerGameDAO.saveChallengeResponse(Challenge);
        }

        public void loadGameData()
        {
            fullQuizList = questionDao.loadQuestionsOfQuiz(base.Quiz.Id);
        }

        private List<Question> filterQuestions(List<Round> rounds)
        {
            List<Question> questions = new List<Question>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var question = it.Current.Question;
                var it2 = fullQuizList.GetEnumerator();
                while (it2.MoveNext())
                {
                    var questionWithAnswer = it2.Current;
                    if (question.QuestionString.Equals(questionWithAnswer.QuestionString))
                    {
                        question.Answers = questionWithAnswer.Answers;
                        fullQuizList.Remove(questionWithAnswer);
                    }
                }
                questions.Add(question);
            }
            return questions;
        }
    }
}
