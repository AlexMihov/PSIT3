﻿using Xunit;
using Moq;
using Quizio.DAO;
using Quizio.Models;
using System.Collections.Generic;
using Quizio.Aggregators;

namespace Quizio.Tests.Integrationtests
{
    /// <summary>
    /// The RankingIntegrationTest is used to test Rankings from Aggregator tier to Data Access tier,
    /// the Data Access Layer is mocked.
    /// </summary>
    public class RankingIntegrationTest
    {
        /// <summary>
        /// testLoadRankings tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Ranking
        /// </summary>
        [Fact]
        public void testLoadRankings()
        {
            // setup for testLoadRankings
            List<Ranking> rankings = new List<Ranking>();
            rankings.Add(new Ranking(1, "spieler 1", 100));
            rankings.Add(new Ranking(2, "spieler 2", 100));
            rankings.Add(new Ranking(3, "spieler 3", 100));

            var rankingDaoMock = new Mock<IRankingDAO>();
            rankingDaoMock.Setup(f => f.loadRankings()).Returns(rankings);

            IRankingDAO rankingDao = rankingDaoMock.Object;

            ModelAggregator aggregator = new ModelAggregator(null, rankingDao, null, null);
            aggregator.Rankings = null;
            // endsetup

            // load rankings with aggregator
            aggregator.loadRankings(); 

            // verify the method is called once
            rankingDaoMock.Verify(f => f.loadRankings(), Times.Once());
            
            // finally assert the loaded rankings to be the same as returned by the dao
            Assert.Equal(aggregator.Rankings, rankings);
        }

        /// <summary>
        /// testRankingUpdate tests from SoloGameAggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: SoloGameAggregator
        /// </summary>
        [Fact]
        public void testRankingUpdate()
        {
            // setup for RankingDaoMock
            User testUser = new User(1, "test", "teststatus", "Visual Studio", "test@test.ch");
            int pointsToAdd = 100;

            var rankingDaoMock = new Mock<IRankingDAO>();

            IRankingDAO rankingDao = rankingDaoMock.Object;

            SoloGameAggregator gameAggregator = new SoloGameAggregator(null, rankingDao);
            gameAggregator.User = testUser;
            // endsetup

            // invoke update from aggregator
            gameAggregator.updateRanking(pointsToAdd); 

            // verify the parameters given by aggregator
            // and make sure the method is called once
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
