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
        public int Id { get; set; }
        public string Name { get; set; }

        public Quiz(string name, List<Question> questions, int id) 
        {
            this.Name = name;
            this.Questions = questions;
            this.Id = id;
        }

        public Quiz(string name, List<Question> questions) : this(name, questions, 0){ }

        public Quiz(string name, int id) : this(name, new List<Question>(), id){} 

        public Question getRandomQuestion()
        {
            int size = Questions.Count;
            var rnd = new Random();
            int num = rnd.Next(1, size);
            return Questions.ElementAt(num);
        }
    }
}
