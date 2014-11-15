using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Category
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; } 
        public List<Quiz> Quizies { get; set; }

        public Category(string name, string description, List<Quiz> quizies)
        {
            this.Name = name;
            this.Description = description;
            this.Quizies = quizies;
        }

        public Category(string name, List<Quiz> quizies) : this(name, "", quizies) { }
    }
}
