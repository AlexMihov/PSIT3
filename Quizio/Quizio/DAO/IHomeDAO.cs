using Quizio.Models;
using System;
using System.Collections.Generic;
namespace Quizio.DAO
{
    public interface IHomeDAO
    {
        List<Notification> loadNotifications(int userID);
        List<Challenge> loadChallenges(int userID);
    }
}
