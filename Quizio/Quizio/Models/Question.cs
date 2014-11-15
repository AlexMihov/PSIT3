using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Question
    {
        
        public int Id { get; set; }
        public string Hint { get; set; }
        public string QuestionString { get; set; }
        public List<Answer> Answers { get; set; }

        public Question(List<Answer> answers, string hint, string question, int id) 
        {
            Id = id;
            Hint = hint;
            QuestionString = question;
            Answers = answers;
        }

        public Question(List<Answer> answers, string hint, string question)
            : this(answers, hint, question, 0)
        {
            this.Answers = answers;
            this.Hint = hint;
            this.QuestionString = question;
        }
    }
}
