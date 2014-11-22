using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public string FromFriend { get; set; } 

        public Notification(string message, string fromFriend)
        {
            this.Message = message;
            this.FromFriend = fromFriend;
        }
    }
}
