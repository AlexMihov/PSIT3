using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Utilities
{
    public class NotificationDAO
    {
        public NotificationDAO()
        {
        }

        public virtual List<Notification> loadNotifications(int UserID)
        {
            string getReq = REST.APIURL + "/notifications";
            string json = REST.get(getReq);
            List<Notification> notifications = JsonConvert.DeserializeObject<List<Notification>>(json);
            return notifications;
        }
    }
}
