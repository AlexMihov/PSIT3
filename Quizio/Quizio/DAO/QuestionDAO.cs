using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    /// <summary>
    /// A class which gets or sets the data related to Question from/into the Database
    /// </summary>
    public class QuestionDAO : IQuestionDAO
    {
        /// <summary>
        /// Gets the Questions of a Quiz
        /// </summary>
        /// <param name="quizID">Unique ID of the quiz whose answers will we fetched</param>
        /// <returns></returns>
        public List<Question> loadQuestionsOfQuiz(int quizID)
        {
            string getReq = REST.APIURL + "/quiz/" + quizID + "/questions";
            string json = REST.get(getReq);

            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(json);
            return questions;
        }
    }
}
