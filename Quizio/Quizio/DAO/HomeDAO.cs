using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public class HomeDAO : IHomeDAO
    {
        public List<Notification> loadNotifications()
        {
            string getReq = REST.APIURL + "/notifications";
            string json = REST.get(getReq);
            List<Notification> notifications = JsonConvert.DeserializeObject<List<Notification>>(json);
            return notifications;
        }


        public List<Challenge> loadChallenges()
        {
            string req = REST.APIURL + "/challenges";
            //string json = REST.get(req);

            //List<Challenge> challengesTemp = JsonConvert.DeserializeObject<List<Challenge>>(json);


            List<Round> input = new List<Round>();
            List<Question> questions = new List<Question>();

            Quiz quiz = new Quiz(0, "Programming Languages", "All About Programming", questions);

            string category = "Computer Science";

            Friend myFriend = new Friend(0, "HulkHogan", "Hogan.js is so cool", "USA");

            int time = 30;

            Challenge first = new Challenge(1, myFriend, quiz, null, input, time, category,
                "Hey buddy here comes Hogan, wanna play?");
            Challenge second = new Challenge(2, myFriend, quiz, null, input, time, category,
                "Just want to give you another chance :D");

            List<Challenge> challenges = new List<Challenge>();
            challenges.Add(first);
            challenges.Add(second);

            return challenges;
        }
    }
}
