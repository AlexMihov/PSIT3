using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Answer
    {
        public bool IsTrue { get; set; }

        public string AnswerText { get; set; }

        public Answer(bool isTrue, string answer)
        {
            this.IsTrue = isTrue;
            this.AnswerText = answer;
        }
    }
}
