namespace Quizio.Models
{
    /// <summary>
    /// The class UserInput is used to save an answer given by the user to a given question. It saves the question, the given answer, the correct answer and if the question given was correct.
    /// </summary>
    public class UserInput
    {
        /// <summary>
        /// Gets or sets the text of the question answered. 
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Gets or sets the Answer the User has given.
        /// </summary>
        public Answer GivenAnswer { get; set; }

        /// <summary>
        /// Gets or sets the correct answer to the question.
        /// </summary>
        public Answer CorrectAnswer { get; set; }

        /// <summary>
        /// Gets and sets a value indicating whether this is the correct answer was given.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the answer given was correct; otherwise, <c>false</c>.
        /// </returns>
        public bool WasCorrect { get; set; }

        /// <summary>
        /// Constructor of the UserInput class, it takes a question, given answer and the correct answer.
        /// </summary>
        /// <param name="question">The question answered by the user as a <c>Question</c></param>
        /// <param name="given">The answer given by the user as a <c>Answer</c></param>
        /// <param name="correctAnswer">The correct answer of the question as a <c>Answer</c></param>
        public UserInput(Question question, Answer given, Answer correctAnswer)
        {
            this.Question = question;
            this.GivenAnswer = given;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
