using Quizio.Models;
using System;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public interface IUserDAO
    {
        void addNewFriend(int friendId);
        void changePassword(string pw);
        void deleteFriend(int friendId);
        List<Friend> loadFriends();
        User logIn(string name, string password);
        void logOut();
        string registerUser(string username, string password, string email, string status, string region);
        List<Friend> searchFriends(string name);
        void updateUserSettings(User user);
    }
}
