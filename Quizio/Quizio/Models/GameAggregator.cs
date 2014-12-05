using Microsoft.Practices.Prism.Mvvm;
using Quizio.DAO;

namespace Quizio.Models
{
    public class GameAggregator : BindableBase
    {
        private Quiz _quiz;
        public Quiz Quiz
        {
            get { return this._quiz; }
            set { SetProperty(ref this._quiz, value); }
        }

        public User User { get; set; }

        #region DAO Interfaces
        private IQuestionDAO questionDao;
        private IRankingDAO rankingDao;
        #endregion

        public GameAggregator()
        {
            // create a new instance of swappable DAO and assign them to the private Interfaces
            questionDao = new QuestionDAO();
            rankingDao = new RankingDAO();
        }

        public GameAggregator(IQuestionDAO questionDao, IRankingDAO rankingDao)
        {
            // assign swappable DAO to the private Interfaces
            this.questionDao = questionDao;
            this.rankingDao = rankingDao;
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
