using Newtonsoft.Json;
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
        public string Title { get; set; }
        public string Description { get; set; }

        public Quiz(int id, string title, string description, List<Question> questions) 
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Questions = questions;
        }

        public Quiz(string title, string description, List<Question> questions) : this(0, title, description, questions){ }

        [JsonConstructor]
        public Quiz(int id, string title, string description) : this(id, title, description, new List<Question>()){}

        public Quiz(string title, List<Question> questions) : this(0, title, "", questions) { }

        public Question getRandomQuestion()
        {
            int size = Questions.Count;
            var rnd = new Random();
            int num = rnd.Next(0, size);
            if (size == 1)
                num = 0;
            return Questions.ElementAt(num);
        }
    }
}