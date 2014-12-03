using Xunit;
using Moq;
using Quizio.Models;
using System.Collections.Generic;
using Quizio.Utilities;

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

            var categoryDaoMock = new Mock<CategoryDAO>();
            categoryDaoMock.Setup(f => f.loadCategories()).Returns(categories);

            CategoryDAO catDao = categoryDaoMock.Object;

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
    }
}
