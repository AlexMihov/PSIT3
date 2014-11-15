using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Answer
    {

        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsTrue { get; set; }

        [JsonConstructor]
        public Answer(int id, string answer, bool value)
        {
            this.IsTrue = value;
            this.Id = id;
            this.AnswerText = answer;
        }

        public Answer(string answer, bool value) : this(0, answer, value) { }
        //public Answer(int id, string answer, int value) : this(id, answer, Convert.ToBoolean(value)) { }
    }
}
