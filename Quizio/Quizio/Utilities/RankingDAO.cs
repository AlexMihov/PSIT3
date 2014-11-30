using Newtonsoft.Json;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Utilities
{
    class RankingDAO
    {
        public RankingDAO()
        {
            //constructor empty
        }

        public List<Ranking> loadRankings(){
            string getReq = REST.APIURL + "/rankings";
            string json = REST.get(getReq);
            List<Ranking> rankings = JsonConvert.DeserializeObject<List<Ranking>>(json);
            return rankings;
        }

        public void updateRanking(User fromUser, int pointsToAdd)
        {
            string putReq = REST.APIURL + "/ranking";
            string json = "{\"toAdd\":" + pointsToAdd + "}";
            REST.put(putReq, json);
        }
    }
}
