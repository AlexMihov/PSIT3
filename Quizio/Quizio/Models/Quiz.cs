using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Quiz
    {
        public List<Question> Questions { get; set; }

        public string QuizName { get; set; }

        public Quiz(string quizName, List<Question> questions)
        {
            this.QuizName = quizName;
            this.Questions = questions;
        }

        public Question getRandomQuestion()
        {
            int size = Questions.Count;
            var rnd = new Random();
            int num = rnd.Next(1, size);
            return Questions.ElementAt(num);
        }
    }
}
