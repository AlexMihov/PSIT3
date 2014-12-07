using Quizio.Models;
using System;
using System.Collections.Generic;
namespace Quizio.DAO
{
    public interface IHomeDAO
    {
        List<Notification> loadNotifications();
        List<Challenge> loadChallenges();
        List<Challenge> loadOpenChallenges();
    }
}
