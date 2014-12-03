using Newtonsoft.Json;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Utilities
{
    public class QuestionDAO
    {
        public virtual List<Question> loadQuestionsOfQuiz(int quizID)
        {
            string getReq = REST.APIURL + "/quiz/" + quizID + "/questions";
            string json = REST.get(getReq);

            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(json);
            return questions;
        }
    }
}
