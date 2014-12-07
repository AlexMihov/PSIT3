using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class ResponseGameAggregator : MultiplayerGameAggregator
    {
        public Player Challenger { get; set; }
        public int ChallangerTime { get; set; }

        public ResponseGameAggregator(Game game) : base()
        {
            base.Quiz = game.PlayedQuiz;
            base.Quiz.Questions = filterQuestions(game.Rounds);
            Challenger = game.Player;
            ChallangerTime = game.Time;
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
