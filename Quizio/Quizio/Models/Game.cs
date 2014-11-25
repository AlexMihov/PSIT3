using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Game
    {
        public List<Question> Questions { get; set; }

        private QuestionDAO questionDao;
        private RankingDAO rankingDao;

        public Game()
        {
            questionDao = new QuestionDAO();
            rankingDao = new RankingDAO();
        }

        public void loadGameData(int id)
        {
            Questions = questionDao.loadQuestionsOfQuiz(id);
        }

        public void updateRanking(User user, int pointsToAdd)
        {
            rankingDao.updateRanking(user, pointsToAdd);
        }
    }
}
