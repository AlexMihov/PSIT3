using Xunit;
using Moq;
using System.Collections.Generic;
using Quizio.Models;
using Quizio.Utilities;

namespace Quizio.Tests.Integrationtests
{
    /// <summary>
    /// The FriendIntegrationTest is used to test Rankings from Aggregator tier to Data Access tier,
    /// the Data Access Layer is mocked.
    /// </summary>
    public class FriendIntegrationTest
    {
        /// <summary>
        /// testLoadFriends tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Friend
        /// </summary>
        [Fact]
        public void testLoadFriends()
        {
            // setup for testLoadFriends
            User testUser = new User(1, "test", "teststatus", "Visual Studio", "test@test.ch");

            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend(2, "testFriend", "testFriendstatus", "Visual Studio"));

            var userDaoMock = new Mock<UserDAO>();
            userDaoMock.Setup(f => f.loadFriends(It.IsAny<int>())).Returns(friends);

            UserDAO userDao = userDaoMock.Object;
            ModelAggregator aggregator = new ModelAggregator(null, null, null, userDao);
            aggregator.User = testUser;
            // endsetup

            // load Friends from aggregator
            aggregator.loadFriends();

            // verify the userid parameter which is given by the aggregator
            // and make sure the method is called once
            userDaoMock.Verify(
                f => f.loadFriends(
                    It.Is<int>(i => i == testUser.Id)),
                    Times.Once()
            );

            // finally assert the loaded friends to be the same as returned from the dao
            Assert.Equal(aggregator.Friends, friends);
        }
    }
}
