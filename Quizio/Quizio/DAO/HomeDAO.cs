using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public class HomeDAO : IHomeDAO
    {
        public List<Notification> loadNotifications(int userID)
        {
            string getReq = REST.APIURL + "/notifications";
            string json = REST.get(getReq);
            List<Notification> notifications = JsonConvert.DeserializeObject<List<Notification>>(json);
            return notifications;
        }


        public List<Challenge> loadChallenges(int userID)
        {
            List<UserInput> input = new List<UserInput>();
            List<Question> questions = new List<Question>();

            Quiz quiz = new Quiz(0, "Programming Languages", "All About Programming", questions);

            string category = "Computer Science";

            Friend myFriend = new Friend(0, "HulkHogan", "Hogan.js is so cool", "USA");

            Challenge first = new Challenge(1, myFriend, quiz, input, category,
                "Hey buddy here comes Hogan, wanna play?");
            Challenge second = new Challenge(2, myFriend, quiz, input, category,
                "Just want to give you another chance :D");

            List<Challenge> challenges = new List<Challenge>();
            challenges.Add(first);
            challenges.Add(second);

            return challenges;
        }
    }
}
