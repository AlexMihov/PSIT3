using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    /// <summary>
    /// A class used to communicate with the database which gets or puts data related to the Homescreen
    /// </summary>
    public class HomeDAO : IHomeDAO
    {
        /// <summary>
        /// Gets all notifications which a user currently has
        /// </summary>
        /// <returns><c>List</c> with all the <c>Notifications</c> of the user.</returns>
        public List<Notification> loadNotifications()
        {
            string getReq = REST.APIURL + "/notifications";
            string json = REST.get(getReq);
            List<Notification> notifications = JsonConvert.DeserializeObject<List<Notification>>(json);
            return notifications;
        }

        /// <summary>
        /// Gets all challanges which a user has accumulated
        /// </summary>
        /// <returns><c>List</c> with all the <c>Challanges</c> of the user.</returns>
        public List<Challenge> loadChallenges()
        {
            string req = REST.APIURL + "/challenges";
            string json = REST.get(req);

            List<Challenge> challenges = JsonConvert.DeserializeObject<List<Challenge>>(json);


            List<Round> input = new List<Round>();
            List<Question> questions = new List<Question>();

            return challenges;
        }

        /// <summary>
        /// Getts all the currently pending challanges a user has.
        /// </summary>
        /// <returns><c>List</c> with all the pending <c>Challanges</c> of the user.</returns>
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
