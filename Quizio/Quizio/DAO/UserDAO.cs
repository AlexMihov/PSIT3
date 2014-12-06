using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Quizio.DAO
{
    /// <summary>
    /// The class is used to talk to the server to load, insert, update or delete data concerning the user data.
    /// </summary>
    public class UserDAO : IUserDAO
    {
        /// <summary>
        /// Sends the login information to the server to establish a new session. Returns
        /// </summary>
        /// <param name="name">Name of the user as a string.</param>
        /// <param name="password">Password of the user as a string.</param>
        /// <returns>All the user information as a user object. Returns null if the user is not found or the password is wrong.</returns>
        public User logIn(string name, string password)
        {
            string req = REST.APIURL + "/login";

            string response = REST.postLogin(req, name, hash(password));

            if (response == "404" || response == "") return null;

            User user = JsonConvert.DeserializeObject<User>(response);
            return user;
        }

        /// <summary>
        /// Loads the list with friends of the user from the server.
        /// </summary>
        /// <returns><c>List</c> with all the <c>friends</c> of the user.</returns>
        public List<Friend> loadFriends()
        {
            string req = REST.APIURL + "/friends";

            string response = REST.get(req);

            List<Friend> friends = JsonConvert.DeserializeObject<List<Friend>>(response);
            return friends;
        }

        /// <summary>
        /// Searches for new friends that have a given string in their name.
        /// </summary>
        /// <param name="name">Part of a name of the user searched for as a string.</param>
        /// <returns>A list of users that match the search criteria and are not yet friends to the user.</returns>
        public List<Friend> searchFriends(string name){
            string req = REST.APIURL + "/player/by-name/" + name;

            string response = REST.get(req);

            List<Friend> friends = JsonConvert.DeserializeObject<List<Friend>>(response);
            return friends;
        }

        /// <summary>
        /// Updates the settings of the user on to the server.
        /// </summary>
        /// <param name="user">User object with the actual data of the user.</param>
        public void updateUserSettings(User user)
        {
            string req = REST.APIURL + "/profile";
            string json = "{ \"name\":\"" + user.Username + "\", \"email\":\"" + user.Email + "\", \"status\": \"" + user.Status + "\", \"location\": \"" + user.Location + "\"}"; ;
            string res = REST.put(req, json);
        }

        /// <summary>
        /// Adds a new friend to the userfriendlist on the server.
        /// </summary>
        /// <param name="friendId">Id of the friend as an integer.</param>
        public void addNewFriend(int friendId)
        {
            string req = REST.APIURL + "/friend";
            string json = "{\"friendId\":" + friendId + "}";
            string res = REST.post(req, json);
        }

        /// <summary>
        /// Deletes a friend of the user on the server.
        /// </summary>
        /// <param name="friendId">Id of the friend as an integer.</param>
        public void deleteFriend(int friendId)
        {
            string req = REST.APIURL + "/friend";
            string json = "{\"friendId\":" + friendId + "}";
            string res = REST.delete(req, json);
        }

        /// <summary>
        /// Changes the password that's saved on the server. The password gets hashed befor it is send to the server.
        /// </summary>
        /// <param name="pw">The new password as a string.</param>
        public void changePassword(string pw)
        {
            string req = REST.APIURL + "/profile/password";
            string json = "{\"password\":" +"\"" + hash(pw) + "\"}";
            string res = REST.put(req, json);
        }

        /// <summary>
        /// Sends information the information of a new user to the server. The password gets hashed befor it's transferd to de server.
        /// </summary>
        /// <param name="username">Username of the registred person as a string</param>
        /// <param name="password">Password as a string. The password gets hashed befor it gets send out.</param>
        /// <param name="email">Email address of the new user as a String</param>
        /// <param name="status">Status of the new user as a string.</param>
        /// <param name="region">Origin of the new user as a string.</param>
        /// <returns>Returns a 200 response if the user has been registred. If the username of the user already exists a 409 http status is returned</returns>
        public string registerUser(string username, string password, string email, string status, string region)
        {

            string req = REST.APIURL + "/player";
            string json = "{ \"name\":\"" + username + "\", \"password\": \"" + hash(password) + "\", \"email\":\"" + email + "\", \"status\": \"" + status + "\", \"origin\":\"" + region + "\"}";

            string response = REST.post(req, json);
            return response;
        }

        private static string hash(string password){
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "dukennschmich"));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// This method sends the server the information that the user has loged out.
        /// </summary>
        public void logOut()
        {
            string req = REST.APIURL + "/logout";
            string response = REST.get(req);

        }
    }
}
