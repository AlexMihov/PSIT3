using Xunit;
using Moq;
using Quizio.Models;
using System.Collections.Generic;
using Quizio.DAO;
using Quizio.Aggregators;

namespace Quizio.Tests.Integrationtests
{
    /// <summary>
    /// The RegularGameIntegrationTest is used to test the GameSelection (Categories)
    /// from Aggregator tier to Data Access tier,
    /// the Data Access Layer is mocked.
    /// </summary>
    public class RegularGameIntegrationTest
    {
        /// <summary>
        /// testLoadCategories tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Category
        /// </summary>
        [Fact]
        public void testLoadCategories()
        {
            // setup for testLoadCategories
            Quiz quiz = new Quiz(1, "Analysis", "Differential, Integration etc.");
            List<Quiz> quizies = new List<Quiz>();
            quizies.Add(quiz);
            List<Category> categories = new List<Category>();
            categories.Add(new Category("Mathematik", "Die abstrakte Sprache der Natur", quizies));

            var categoryDaoMock = new Mock<ICategoryDAO>();
            categoryDaoMock.Setup(f => f.loadCategories()).Returns(categories);

            ICategoryDAO catDao = categoryDaoMock.Object;

            ModelAggregator aggregator = new ModelAggregator(null, null, catDao, null);
            // endsetup

            // Categories should be null
            Assert.True(aggregator.Categories == null);

            // invoke call from aggregator to dao
            aggregator.loadCategories();

            // verify the dao method is called once
            categoryDaoMock.Verify(f => f.loadCategories(), Times.Once());

            // the loaded categories should be the same as returned from the dao
            Assert.Equal(categories, aggregator.Categories);
        }

        /// <summary>
        /// testLoadGameData tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, SoloGameAggregator,
        /// Question and Answer
        /// </summary>
        [Fact]
        public void testLoadGameData()
        {
            //setup for testLoadGameData
            List<Answer> answers = new List<Answer>();
            answers.Add(new Answer("f'(x) = e^x", true));
            answers.Add(new Answer("f'(x) = - e^x/2", false));
            List<Question> questions = new List<Question>();
            questions.Add(new Question(1, "Was ist die Ableitung von F(x) = e^x ?", "e-Fkt.", answers));

            Quiz selectedQuiz = new Quiz(1, "Analysis", "Differential, Integration etc.", new List<Question>());

            var questionDaoMock = new Mock<IQuestionDAO>();
            questionDaoMock.Setup(f => f.loadQuestionsOfQuiz(It.IsAny<int>())).Returns(questions);

            IQuestionDAO questionDao = questionDaoMock.Object;

            SoloGameAggregator gameAggregator = new SoloGameAggregator(questionDao, null);
            //endsetup

            // quiz should be null
            Assert.True(gameAggregator.Quiz == null);

            // call method on gameAggregator to load game data
            gameAggregator.loadGameData(selectedQuiz);

            // verify that the given parameter selectedQuiz from gameAggregator is passed correctly
            questionDaoMock.Verify(
                f => f.loadQuestionsOfQuiz(
                    It.Is<int>(i => i == selectedQuiz.Id)
                    ),
                    Times.Once()
            );

            // finally check the loaded data was loaded correctly by mock dao
            Assert.Equal(selectedQuiz, gameAggregator.Quiz);
            Assert.Equal(questions, gameAggregator.Quiz.Questions);
        }
    }
}
