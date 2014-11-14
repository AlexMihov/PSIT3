using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Question
    {
        public List<Answer> Answers { get; set; }

        public string Hint { get; set; }

        public string QuestionText { get; set; }

        public Question(List<Answer> answers, string hint, string quest )
        {
            this.Answers = answers;
            this.Hint = hint;
            this.QuestionText = quest;
        }
    }
}
