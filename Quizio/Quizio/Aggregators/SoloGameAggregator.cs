using Microsoft.Practices.Prism.Mvvm;
using Quizio.DAO;
using Quizio.Models;
using System.Collections.Generic;
using System.Linq;

namespace Quizio.Aggregators
{
    public class SoloGameAggregator : BindableBase
    {
        private Quiz _quiz;
        public Quiz Quiz
        {
            get { return this._quiz; }
            set { SetProperty(ref this._quiz, value); }
        }

        public User User { get; set; }

        public int TimeNeededSum { get; set; }

        public List<Round> Rounds { get; set; }

        #region DAO Interfaces
        internal IQuestionDAO questionDao;
        internal IRankingDAO rankingDao;
        #endregion

        public SoloGameAggregator()
        {
            // create a new instance of swappable DAO and assign them to the private Interfaces
            questionDao = new QuestionDAO();
            rankingDao = new RankingDAO();

            Rounds = new List<Round>();
        }

        public SoloGameAggregator(IQuestionDAO questionDao, IRankingDAO rankingDao)
        {
            // assign swappable DAO to the private Interfaces
            this.questionDao = questionDao;
            this.rankingDao = rankingDao;
        }

        public virtual void loadGameData(Quiz quiz)
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
