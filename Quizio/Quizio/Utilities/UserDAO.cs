﻿using Newtonsoft.Json;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

            string response = REST.postLogin(req, name, heschi(password));

            if (response == "404" || response == "") return null;

            User user = JsonConvert.DeserializeObject<User>(response);
            return user;
        }

        public List<Friend> loadFriends(int userId)
        {
            string req = REST.APIURL + "/friends";

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

        public void updateUserSettings(User user)
        {
            throw new NotImplementedException();
        }

        public void addNewFriend(int friendId)
        {
            string req = REST.APIURL + "/friend";
            string json = "{\"friendId\":" + friendId + "}";
            string res = REST.post(req, json);
        }

        public void deleteFriend(int friendId)
        {
            string req = REST.APIURL + "/friend";
            string json = "{\"friendId\":" + friendId + "}";
            string res = REST.delete(req, json);
        }

        public void changePassword(string pw)
        {
            Console.WriteLine(pw);
        }

        public void registerUser(string username, string password, string email, string status, string region)
        {

            string req = REST.APIURL + "/player";
            string json = "{ \"name\":\"" + username + "\", \"password\": \"" + heschi(password) + "\", \"email\":\"" + email + "\", \"status\": \"" + status + "\", \"origin\":\"" + region + "\"}";

            string response = REST.post(req, json);
        }

        private static string heschi(string password){
            SHA256 sha256 = SHA256Managed.Create(); //utf8 here as well
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "dukennschmich"));
            return Convert.ToBase64String(bytes);
        }

        internal void logOut()
        {
            throw new NotImplementedException(); // sött so gah wil er kennt ja de user
        }
    }
}
