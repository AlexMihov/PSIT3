using Quizio.Models;
using System;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public interface IQuestionDAO
    {
        List<Question> loadQuestionsOfQuiz(int quizID);
    }
}
