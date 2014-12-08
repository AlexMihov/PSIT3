using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Quizio.Models;
using Quizio.DAO;
using Quizio.Aggregators;

namespace Quizio.Tests.Integrationtests
{
    /// <summary>
    /// The FriendIntegrationTest is used to test Friends from Aggregator tier to Data Access tier,
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

            var userDaoMock = new Mock<IUserDAO>();
            userDaoMock.Setup(f => f.loadFriends()).Returns(friends);

            IUserDAO userDao = userDaoMock.Object;
            ModelAggregator aggregator = new ModelAggregator(null, null, null, userDao);
            aggregator.User = testUser;
            // endsetup

            // selectedfriend should not be set (null)
            Assert.False(aggregator.SelectedFriend != null);

            // load Friends from aggregator
            aggregator.loadFriends();

            // verify the userid parameter which is given by the aggregator
            // and make sure the method is called once
            userDaoMock.Verify(
                f => f.loadFriends(),
                    Times.Once()
            );

            // assert the loaded friends to be the same as returned from the dao
            Assert.Equal(aggregator.Friends, friends);

            // finally check if the SelectedUser is set after loading
            Assert.Equal(aggregator.SelectedFriend, friends.FirstOrDefault());
        }

        /// <summary>
        /// testSearchFriend tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Friend
        /// </summary>
        [Fact]
        public void testSearchFriend()
        {
            // setup for testLoadFriends
            User testUser = new User(1, "test", "teststatus", "Visual Studio", "test@test.ch");

            string toSearch = "test";

            List<Friend> searchResult = new List<Friend>();
            searchResult.Add(new Friend(2, "testFriend1", "testFriendstatus", "Visual Studio"));
            searchResult.Add(new Friend(3, "testFriend2", "testFriendstatus", "Visual Studio"));

            var userDaoMock = new Mock<IUserDAO>();
            userDaoMock.Setup(f => f.searchFriends(It.IsAny<string>())).Returns(searchResult);

            IUserDAO userDao = userDaoMock.Object;
            ModelAggregator aggregator = new ModelAggregator(null, null, null, userDao);
            aggregator.FriendSearch = toSearch;
            // endsetup

            // SearchResult should not be set (null)
            Assert.False(aggregator.SearchResult != null);

            // call aggregator method searchFriends to start the test
            aggregator.searchFriends();

            // verify the friendId parameter which is given by the aggregator
            // and make sure the method is called once
            userDaoMock.Verify(
                f => f.searchFriends(
                    It.Is<string>(s => s.Equals(toSearch))
                    ),
                    Times.Once()
            );

            // finally assert the searchResult matchings for the search string
            // to be the same as returned by the dao
            Assert.Equal(aggregator.SearchResult, searchResult);
        }

        /// <summary>
        /// testAddDelFriends tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Friend
        /// </summary>
        [Fact]
        public void testAddDelFriends()
        {
            // setup for testLoadFriends
            Friend toDelete = new Friend(2, "testFriend1", "testFriendstatus1", "Visual Studio");

            Friend toAdd = new Friend(4, "testFriend3", "testFriendstatus3", "Visual Studio");

            var userDaoMock = new Mock<IUserDAO>();
            userDaoMock.Setup(f => f.addNewFriend(It.IsAny<int>()));
            userDaoMock.Setup(f => f.deleteFriend(It.IsAny<int>()));

            IUserDAO userDao = userDaoMock.Object;
            ModelAggregator aggregator = new ModelAggregator(null, null, null, userDao);
            // endsetup

            // first friend selected
            aggregator.SelectedFriend = toDelete;

            // selectedFriend deleted
            aggregator.deleteFriend();

            // last friend selected
            aggregator.SelectedResult = toAdd;

            // selectedResult added
            aggregator.addFriend();

            // verify the friendId parameter which is given by the aggregator
            // and make sure the method is called once
            userDaoMock.Verify(
                f => f.deleteFriend(
                    It.Is<int>(i => i == toDelete.Id)
                    ),
                    Times.Once()
            );
            userDaoMock.Verify(
                f => f.addNewFriend(
                    It.Is<int>(i => i == toAdd.Id)
                    ),
                    Times.Once()
            );
        }
    }
}
