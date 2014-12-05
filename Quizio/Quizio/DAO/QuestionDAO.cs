using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public class QuestionDAO : IQuestionDAO
    {
        public List<Question> loadQuestionsOfQuiz(int quizID)
        {
            string getReq = REST.APIURL + "/quiz/" + quizID + "/questions";
            string json = REST.get(getReq);

            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(json);
            return questions;
        }
    }
}
