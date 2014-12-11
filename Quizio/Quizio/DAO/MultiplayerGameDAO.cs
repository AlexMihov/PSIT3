using Newtonsoft.Json;
using Quizio.Models;
using Quizio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.DAO
{
    /// <summary>
    /// A class used to get or post information related to the Multiplayer from/into the database
    /// </summary>
    public class MultiplayerGameDAO : IMultiplayerGameDAO
    {
        /// <summary>
        /// Saves a new challange to another player
        /// </summary>
        /// <param name="challengeGame">The gamedata of the played Quiz</param>
        /// <param name="challengedFriend">The player which will be challanged</param>
        /// <param name="challengeText">The text the challanged player will see</param>
        public void saveChallengeGame(Game challengeGame, Player challengedFriend, string challengeText)
        {
            string json = "{ \"text\":\"" + challengeText + "\"";
            json += ", \"challengedPlayer\": " + challengedFriend.ToJson();
            json += ", \"challenge\": " + challengeGame.ToJson();
            json += "}";
            string req = REST.APIURL + "/challenge";
            string response = REST.post(req, json);
        }

        /// <summary>
        /// Saves the response to the Game which the challanged player gives
        /// </summary>
        /// <param name="challenge"></param>
        public void saveChallengeResponse(Challenge challenge)
        {

                string json = "{\"id\": \"" + challenge.Id + "\"";
                json += ", \"text\":\"" + challenge.ChallengeText + "\"";
                json += ", \"status\": \"" + challenge.Status + "\"";
                json += ", \"challengedPlayer\": " + challenge.ChallengedPlayer.ToJson();
                json += ", \"challenge\": " + challenge.ChallengeGame.ToJson();
                if (challenge.ResponseGame != null)
                {    
                    json += ", \"response\": " + challenge.ResponseGame.ToJson();
                }
                json += "}";
                string req = REST.APIURL + "/challenge";
                string response = REST.put(req, json);
            
        }

        /// <summary>
        /// Gets a challange from the server by the unique ID
        /// </summary>
        /// <param name="challengeId">The ID of the challange which will be retrieved from the server</param>
        /// <returns></returns>
        public Challenge getChallenge(int challengeId)
        {
            string req = REST.APIURL + "/challenge/" + challengeId;
            string json = REST.get(req);


            return JsonConvert.DeserializeObject<Challenge>(json);
        }
    }
}
