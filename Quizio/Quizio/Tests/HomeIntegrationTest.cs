using Xunit;
using Moq;
using Quizio.Utilities;
using System.Collections.Generic;
using Quizio.Models;

namespace Quizio.Tests
{
    /// <summary>
    /// The HomeIntegrationTest is used to test Rankings from Aggregator tier to Data Access tier,
    /// the Data Access Layer is mocked.
    /// </summary>
    public class HomeIntegrationTest
    {
        /// <summary>
        /// testLoadNotifications tests from Model Aggregator to Model while
        /// the Data Acces Layer is Mocked, Class under test: ModelAggregator, Notification
        /// </summary>
        [Fact]
        public void testLoadNotifications()
        {
            // setup for testLoadNotifications
            User testUser = new User(1, "test", "teststatus", "Virtual Studio", "test@test.ch");

            List<Notification> notifications = new List<Notification>();
            notifications.Add(new Notification("testmessage", "test"));
            
            var notificationDaoMock = new Mock<NotificationDAO>();
            notificationDaoMock.Setup(f => f.loadNotifications(It.IsAny<int>())).Returns(notifications);

            NotificationDAO natDao = notificationDaoMock.Object;
            ModelAggregator aggregator = new ModelAggregator(natDao, null, null, null);
            aggregator.User = testUser;
            // endsetup

            // load homeView data from aggregator
            aggregator.reloadHomeData();

            // verify the userid parameter given by aggregator 
            // and make sure the method is called once
            notificationDaoMock.Verify(
                f => f.loadNotifications(
                    It.Is<int>(i => i == testUser.Id)
                    ),
                Times.Once()
             );

            // finally assert the loaded notifications to be the same as returned from the dao
            Assert.Equal(aggregator.Notifications, notifications);
        } 
    }
}
