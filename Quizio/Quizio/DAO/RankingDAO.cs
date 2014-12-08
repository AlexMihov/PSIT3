using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public class RankingDAO : IRankingDAO
    {
        public List<Ranking> loadRankings(){
            string getReq = REST.APIURL + "/rankings";
            string json = REST.get(getReq);
            List<Ranking> rankings = JsonConvert.DeserializeObject<List<Ranking>>(json);
            return rankings;
        }

        public void updateRanking(Player fromUser, int pointsToAdd)
        {
            string putReq = REST.APIURL + "/ranking";
            string json = "{\"toAdd\":" + pointsToAdd + ",\"playerId\":" +fromUser.Id+ "}";
            REST.put(putReq, json);
        }
    }
}
