using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        public List<Friend> Friends { get; set; }

        public User(string username)
        {
            this.Username = username;
        }

        public User(string username, string status, List<Friend> friends)
        {
            this.Username = username;
            this.Status = status;
            this.Friends = friends;
        }

        [JsonConstructor]
        public User(string username, string status, string location, string email, List<Friend> friends)
        {
            this.Username = username;
            this.Status = status;
            this.Location = location;
            this.Email = email;
            this.Friends = friends;
        }
    }
}
