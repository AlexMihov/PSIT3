using Newtonsoft.Json;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Utilities
{
    class UserDAO
    {


        public UserDAO()
        {

        }


        public User logIn(string name, string password)
        {
            string req = REST.APIURL + "/login";

            string response = REST.postLogin(req, name, password);

            if (response == "404" || response == "") return null;

            User user = JsonConvert.DeserializeObject<User>(response);
            return user;
        }

        public List<Friend> loadFriends(int userId)
        {
            string req = REST.APIURL + "/friends/" + userId;

            string response = REST.get(req);

            List<Friend> friends = JsonConvert.DeserializeObject<List<Friend>>(response);
            return friends;
        }

        public List<Friend> searchFriends(string name){
            string req = REST.APIURL + "/player/by-name/" + name;

            string response = REST.get(req);

            List<Friend> friends = JsonConvert.DeserializeObject<List<Friend>>(response);
            return friends;
        }
    }
}
