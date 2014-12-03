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
            // endsetup

            RankingDAO rankingDao = rankingDaoMock.Object;

            ModelAggregator aggregator = new ModelAggregator(null, rankingDao, null, null);
            aggregator.Rankings = null; // initialize rankings with null
            aggregator.loadRankings(); // load rankings with mocked DAO

            rankingDaoMock.Verify(f => f.loadRankings(), Times.AtLeastOnce());
            
            Assert.Equal(aggregator.Rankings, rankings);
        }
    }
}
