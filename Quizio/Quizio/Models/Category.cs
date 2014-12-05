using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Quizio.Models
{
    /// <summary>
    /// The class category is a model for quiz categories. It saves a name, an id, a description and a list of quizies.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets the name of a category as an string.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the id of category to interact with the db as an integer.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or set the description of the category as an string.
        /// </summary>
        public string Description { get; set; } 
        
        /// <summary>
        /// Gets or sets a list of <c>Quiz</c> holding all the quizies, which belong to this category.
        /// </summary>
        public List<Quiz> Quizies { get; private set; }

        /// <summary>
        /// Constructor of the category class, it takes an id, a name, a description and a list of quizies. It can be used to create a Category from a json string.
        /// </summary>
        /// <param name="id">The id a category has in the db as an integer</param>
        /// <param name="name">The name of a category as a string</param>
        /// <param name="description">The description of a category as a string</param>
        /// <param name="quizies">A list of quizies as <c>List<c> containing <c>Quiz</c>, which belong to this category.</Quiz></c></Quiz></c></param>
        [JsonConstructor]
        public Category(int id, string name, string description, List<Quiz> quizies) 
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Quizies = quizies;
        }

        /// <summary>
        /// A short constructor of the category class, it takes a name, a description and a list of quizies. The id is automaticly set to zero.
        /// </summary>
        /// <param name="name">The name of a category as a string</param>
        /// <param name="description">The description of a category as a string</param>
        /// <param name="quizies">A list of quizies as <c>List<c> containing <c>Quiz</c>, which belong to this category.</param>
        public Category(string name, string description, List<Quiz> quizies) : this(0, name, description, quizies) { }
    }
}