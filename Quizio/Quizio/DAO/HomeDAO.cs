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

            Category category = new Category("Computer Science", "All about computer Science", null);

            Friend myFriend = new Friend(1, "HulkHogan", "Hogan.js is so cool", "USA");

            int time = 30;


            Game firstG = new Game(1, myFriend, time, input, quiz, category);

            Challenge first = new Challenge(1, firstG, null, "Hey buddy here comes Hogan, wanna play?");



            Game secondG = new Game(2, myFriend, time, input, quiz, category);

            Challenge second = new Challenge(2, secondG, null, "Just want to give you another chance :D");

            List<Challenge> challenges = new List<Challenge>();
            challenges.Add(first);
            challenges.Add(second);

            return challenges;
        }
    }
}
