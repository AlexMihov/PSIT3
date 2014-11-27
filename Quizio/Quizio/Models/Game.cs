using Microsoft.Practices.Prism.Mvvm;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Game : BindableBase
    {
        private Quiz _quiz;
        public Quiz Quiz
        {
            get { return this._quiz; }
            set { SetProperty(ref this._quiz, value); }
        }

        public User User { get; set; }

        #region DAO's
        private QuestionDAO questionDao;
        private RankingDAO rankingDao;
        #endregion

        public Game()
        {
            questionDao = new QuestionDAO();
            rankingDao = new RankingDAO();
        }

        public void loadGameData(Quiz quiz)
        {
            this.Quiz = quiz;
            Quiz.Questions = questionDao.loadQuestionsOfQuiz(quiz.Id);
        }

        public void updateRanking(int pointsToAdd)
        {
            rankingDao.updateRanking(User, pointsToAdd);
        }
    }
}
