using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    /// <summary>
    /// A class used to get or set data associated with the Ranking from/into the database
    /// </summary>
    public class RankingDAO : IRankingDAO
    {
        /// <summary>
        /// Gets the current ranking from the Database
        /// </summary>
        /// <returns><c>List</c> with all the <c>Rankings</c> of the users.</returns>
        public List<Ranking> loadRankings(){
            string getReq = REST.APIURL + "/rankings";
            string json = REST.get(getReq);
            List<Ranking> rankings = JsonConvert.DeserializeObject<List<Ranking>>(json);
            return rankings;
        }
        /// <summary>
        /// Updates a Ranking by adding points to a user
        /// </summary>
        /// <param name="fromUser">The user whose ranking will be updated</param>
        /// <param name="pointsToAdd">The ammount of points which the usser has recieved and which need to be added to the current points</param>
        public void updateRanking(Player fromUser, int pointsToAdd)
        {
            string putReq = REST.APIURL + "/ranking";
            string json = "{\"toAdd\":" + pointsToAdd + ",\"playerId\":" +fromUser.Id+ "}";
            REST.put(putReq, json);
        }
    }
}
