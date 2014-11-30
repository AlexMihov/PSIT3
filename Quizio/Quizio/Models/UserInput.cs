using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class UserInput
    {
        public string QuestionText { get; set; }

        public Answer GivenAnswer { get; set; }

        public Answer CorrectAnswer { get; set; }

        public bool WasCorrect { get; set; }

        public UserInput(string text, Answer given, Answer correct)
        {
            this.QuestionText = text;
            this.GivenAnswer = given;
            this.CorrectAnswer = correct;
            if (given.Equals(correct))
            {
                this.WasCorrect = true;
            }
            else
            {
                this.WasCorrect = false;
            }
        }
    }
}
