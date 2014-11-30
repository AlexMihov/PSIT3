using Newtonsoft.Json;

namespace Quizio.Models
{
    /// <summary>
    /// The class answer is a model for an answer of a question. It can contain an id, answer text and a value that shows if the answer is correct.
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Gets or sets the id that an answer has in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text of the answer as a string.
        /// </summary>
        public string AnswerText { get; set; }
        
        /// <summary>
        /// Gets and sets a value indicating whether this is the correct answer.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the answer is correct; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTrue { get; set; }

        /// <summary>
        /// Constructor of the the answer model. It can be used to create a answer model from a json string.
        /// </summary>
        /// <param name="id">The id an answer has in the db as an integer</param>
        /// <param name="answer">The answer text as a string</param>
        /// <param name="value">The value of the answer as a bool, <c>true</c> if the answer is correct, else <c>false</c></param>
        [JsonConstructor]
        public Answer(int id, string answer, bool value)
        {
            this.IsTrue = value;
            this.Id = id;
            this.AnswerText = answer;
        }

        /// <summary>
        /// Simple constructor of the answer model, that doesn't require an database id, it sets the id to zero.
        /// </summary>
        /// <param name="answer">The answer text as a string</param>
        /// <param name="value">The value of the answer as a bool, <c>true</c> if the answer is correct, else <c>false</c></param>
        public Answer(string answer, bool value) : this(0, answer, value) { }
    }
}