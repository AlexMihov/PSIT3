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
            string json = REST.get(req);

            List<Challenge> challenges = JsonConvert.DeserializeObject<List<Challenge>>(json);


            List<Round> input = new List<Round>();
            List<Question> questions = new List<Question>();

            return challenges;
        }


        public List<Challenge> loadOpenChallenges()
        {
            string req = REST.APIURL + "/challenges/offen";
            string json = REST.get(req);

            List<Challenge> challenges = JsonConvert.DeserializeObject<List<Challenge>>(json);


            List<Round> input = new List<Round>();
            List<Question> questions = new List<Question>();

            return challenges;
        }
    }
}
