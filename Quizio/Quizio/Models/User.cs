using Newtonsoft.Json;
using Quizio.Utilities;
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

        //public List<Friend> Friends;

        public User()
        {

        }

        public User(string username)
        {
            this.Username = username;
        }

        public User(string username, string status)
        {
            this.Username = username;
            this.Status = status;
        }

        [JsonConstructor]
        public User(int id, string name, string status, string location, string email)
        {
            this.Id = id;
            this.Username = name;
            this.Status = status;
            this.Location = location;
            this.Email = email;
        }



    }
}
