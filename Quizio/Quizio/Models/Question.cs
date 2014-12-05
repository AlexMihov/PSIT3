using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Quizio.Models
{
    /// <summary>
    /// The class question is a model for questions of a quiz. It saves an id, a hint, a question as a string and a list of answers.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets or sets the id that a question has in the database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets a hint for the question as a string.
        /// </summary>
        public string Hint { get; set; }
        
        /// <summary>
        /// Gets or sets a question as a string
        /// </summary>
        public string QuestionString { get; set; }
        
        /// <summary>
        /// Gets or sets a list of answers to the question.
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// Constructor of the question class, it takes an id, a question, a hint and a list of answer. It can be used to create a question model from a json string.
        /// </summary>
        /// <param name="id">The id a question has in the db as an integer</param>
        /// <param name="question">The question text as a string</param>
        /// <param name="hint">A hint for the question as a string</param>
        /// <param name="answers">A <c>List</c> of <c>Answer</c> that are possible result to the question"</param>
        [JsonConstructor]
        public Question(int id, string question, string hint, List<Answer> answers) 
        {
            Id = id;
            Hint = hint;
            QuestionString = question;
            Answers = answers;
        }

        /// <summary>
        /// Determines which of the answers is the correct one and returns it.
        /// </summary>
        /// <returns>The <c>Answer</c> that contains <c>true</c> as a value.</returns>
        public Answer GetCorrectAnswer()
        {
            Answer toReturn = Answers.FirstOrDefault();
            IEnumerator<Answer> it = Answers.GetEnumerator();
            while (it.MoveNext())
            {
                Answer currentAnswer = it.Current;
                if (currentAnswer.IsTrue == true)
                {
                    toReturn = currentAnswer;
                    break;
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Finds a specific answer of the question by the id of the answer.
        /// </summary>
        /// <param name="answerText">Text of the answer as string</param>
        /// <returns>The <c>Answer</c> that contains the answer text</returns>
        public Answer GetAnswerById(int id)
        {
            IEnumerator<Answer> it = Answers.GetEnumerator();
            while (it.MoveNext())
            {
                Answer currentAnswer = it.Current;
                if (it.Current.Id == id)
                {
                    return currentAnswer;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a specific answer of the question by the text of the answer.
        /// </summary>
        /// <param name="answerText">Text of the answer as string</param>
        /// <returns>The <c>Answer</c> that contains the answer text</returns>
        public Answer GetAnswerByText(string answerText)
        {
            Answer toReturn = Answers.FirstOrDefault();
            IEnumerator<Answer> it = Answers.GetEnumerator();
            while (it.MoveNext())
            {
                Answer currentAnswer = it.Current;
                if (currentAnswer.IsTrue && currentAnswer.AnswerText.Equals(answerText))
                {
                    toReturn = currentAnswer;
                    break;
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Looks at an answer to the and checks if it is correct.
        /// </summary>
        /// <param name="answerText">Text of the answer as string</param>
        /// <returns>Returns <c>true</c> if the answer is correct else returns <c>false</c></returns>
        public bool checkAnswer(string answerText)
        {
            bool toReturn = false;
            IEnumerator<Answer> it = Answers.GetEnumerator();
            while (it.MoveNext())
            {
                Answer currentAnswer = it.Current;
                if (currentAnswer.IsTrue && currentAnswer.AnswerText.Equals(answerText))
                {
                    toReturn = true;
                    return toReturn;
                }
            }
            return toReturn;
        }
    }
}