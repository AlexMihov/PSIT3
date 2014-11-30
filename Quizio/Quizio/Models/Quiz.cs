using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quizio.Models
{
    /// <summary>
    /// The class quiz is a model for a quiz with an id, a title, a description as a string and a list of questions.
    /// </summary>
    public class Quiz
    {
        /// <summary>
        /// Gets or sets the id that a question has in the database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets a title for the quiz as a string.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets a description for the quiz as a string.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets a list of questions of the quiz.
        /// </summary>
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Constructor of the quiz class, it takes an id, a title, a description and a <c>List</c> of <c>Question</c> belonging to the quiz.
        /// </summary>
        /// <param name="id">The id a question has in the db as an integer</param>
        /// <param name="title">The title of a quiz as a string</param>
        /// <param name="description">The title of a quiz as a string</param>
        /// <param name="questions">A <c>List</c> of <c>Question</c> that belong to the quiz"</param>
        public Quiz(int id, string title, string description, List<Question> questions) 
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Questions = questions;
        }

        /// <summary>
        /// The JSON constructor of the quiz class, it takes an id, a title and a description. An empty list of question is generated. The constructor can be used to create a question model from a json string.
        /// </summary>
        /// <param name="id">The id a question has in the db as an integer</param>
        /// <param name="title">The title of a quiz as a string</param>
        /// <param name="description">The description of a quiz as a string</param>
        [JsonConstructor]
        public Quiz(int id, string title, string description) : this(id, title, description, new List<Question>()){}

        /// <summary>
        /// Returns a random question saved in this quiz.
        /// </summary>
        /// <returns><c>Question</c>that is randomly selected from the quiz</returns>
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