using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.DAO
{
    public class MultiplayerGameDAO : IMultiplayerGameDAO
    {

        public void saveChallengeGame(Game challengeGame, Player challengedFriend, string challengeText)
        {
            string json = "{ \"text\":\"" + challengeText + "\"";
            json += ", \"challengedPlayer\": " + challengedFriend.ToJson();
            json += ", \"challenge\": " + challengeGame.ToJson();
            json += "}";
            string req = REST.APIURL + "/challenge";
            string response = REST.post(req, json);
        }


        public void saveChallengeResponse(Challenge challenge)
        {
            //save
        }
    }
}
