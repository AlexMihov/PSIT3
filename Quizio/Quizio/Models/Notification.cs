using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Notification
    {
        /// <summary>
        /// Gets or sets the Message which the Notification holds
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the Name of the player which generated this Notification
        /// </summary>
        public string FromFriend { get; set; } 
        /// <summary>
        /// Constructor for Notifications which are displayed
        /// </summary>
        /// <param name="message">The text of the notification</param>
        /// <param name="fromFriend">The name of the Player which created this notification</param>
        public Notification(string message, string fromFriend)
        {
            this.Message = message;
            this.FromFriend = fromFriend;
        }
    }
}