using Newtonsoft.Json;
namespace Quizio.Models
{
    /// <summary>
    /// The class Round is used to save an answer given by the user to a given question. It saves the question, the given answer, the correct answer and if the question given was correct.
    /// </summary>
    public class Round
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
        /// Constructor of the Round class, it takes a question and the id of the given answer. 
        /// The other information is extracted in the contructor from the question object. 
        /// </summary>
        /// <param name="question">The question answered by the user as a <c>Question</c></param>
        /// <param name="givenAnswerId">The id of the answer given by the user as an interger</param>
        [JsonConstructor]
        public Round(Question question, int givenAnswerId)
        {
            if (givenAnswerId == 0 || givenAnswerId == 1 || givenAnswerId == null)
            {
                this.GivenAnswer = new Answer(1, "Zeit abgelaufen", false);
            } 
            else 
            {
                this.GivenAnswer = question.GetAnswerById(givenAnswerId);
            }
            this.Question = question;
            this.CorrectAnswer = question.GetCorrectAnswer();
        }

        public bool isCorrect()
        {
            return (GivenAnswer.Equals(CorrectAnswer));
        }

        public bool isTimedOut()
        {
            if(GivenAnswer == null) return true;
            if (GivenAnswer.Id == 0 || GivenAnswer.Id == 1) return true;
            else return false;
        }


        public string ToJson()
        {
            return "{\"questionId\": " + Question.Id + "," + "\"answerId\":" + GivenAnswer.Id + "}";
        }

    }
}
