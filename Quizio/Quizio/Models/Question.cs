using Newtonsoft.Json;
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

        [JsonConstructor]
        public Question(int id, string question, string hint, List<Answer> answers) 
        {
            Id = id;
            Hint = hint;
            QuestionString = question;
            Answers = answers;
        }

        public Question(List<Answer> answers, string hint, string question)
            : this(0, question, hint, answers)
        {
            this.Answers = answers;
            this.Hint = hint;
            this.QuestionString = question;
        }
    }
}
