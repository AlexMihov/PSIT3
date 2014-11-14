using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class User
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public List<Friend> Friends { get; set; }

        public User(string username, string loc, string stat, List<Friend> friends)
        {
            this.Username = username;
            this.Location = loc;
            this.Status = stat;
            this.Friends = friends;
        }
    }
}
