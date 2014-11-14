using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Category
    {
        public string CategoryName { get; set; }

        public List<Quiz> Quizies { get; set; }

        public Category(string categoryName, List<Quiz> quizies)
        {
            this.CategoryName = categoryName;
            this.Quizies = quizies;
        }
    }
}
