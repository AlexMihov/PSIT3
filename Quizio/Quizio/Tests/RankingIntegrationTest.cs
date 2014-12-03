using Xunit;
using Moq;
using Quizio.Utilities;
using Quizio.Models;
using System.Collections.Generic;

namespace Quizio.Tests
{
    public class RankingIntegrationTest
    {
        /// <summary>
        /// testLoadRankings tests from Model Aggregator to Model
        /// Data Acces Layer is Mocked, Class under test: ModelAggregator, Ranking
        /// </summary>
        [Fact]
        public void testLoadRankings()
        {
            // setup for RankingDaoMock
            List<Ranking> rankings = new List<Ranking>();
            rankings.Add(new Ranking(1, "spieler 1", 100));
            rankings.Add(new Ranking(2, "spieler 2", 100));
            rankings.Add(new Ranking(3, "spieler 3", 100));

            var rankingDaoMock = new Mock<RankingDAO>();
            rankingDaoMock.Setup(f => f.loadRankings()).Returns(rankings);

            RankingDAO rankingDao = rankingDaoMock.Object;

            ModelAggregator aggregator = new ModelAggregator(null, rankingDao, null, null);
            aggregator.Rankings = null;
            // endsetup

            aggregator.loadRankings(); // load rankings with mocked DAO

            rankingDaoMock.Verify(f => f.loadRankings(), Times.Once());
            
            Assert.Equal(aggregator.Rankings, rankings);
        }

        /// <summary>
        /// testRankingUpdate tests from GameAggregator to Model
        /// Data Acces Layer is Mocked, Class under test: GameAggregator
        /// </summary>
        [Fact]
        public void testRankingUpdate()
        {
            // setup for RankingDaoMock
            User testUser = new User(1, "test", "teststatus", "Virtual Studio", "test@test.ch");
            int pointsToAdd = 100;

            var rankingDaoMock = new Mock<RankingDAO>();

            RankingDAO rankingDao = rankingDaoMock.Object;

            GameAggregator gameAggregator = new GameAggregator(null, rankingDao);
            gameAggregator.User = testUser;
            // endsetup

            gameAggregator.updateRanking(pointsToAdd); // invoke update from aggregator

            // verify the rankingDao parameters, given by aggregator
            rankingDaoMock.Verify(
                f => f.updateRanking(
                It.Is<User>(u => u.Equals(testUser)),
                It.Is<int>(i => i == pointsToAdd)
                ),
                Times.Once()
            );
        }
    }
}
