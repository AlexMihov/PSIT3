using Quizio.Models;
using System;
using System.Collections.Generic;
namespace Quizio.DAO
{
    public interface INotificationDAO
    {
        List<Notification> loadNotifications(int UserID);
    }
}
