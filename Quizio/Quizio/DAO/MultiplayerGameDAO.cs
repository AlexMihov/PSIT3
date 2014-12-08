﻿using Newtonsoft.Json;
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
            if (challenge.ResponseGame != null)
            {
                string json = "{\"id\": \"" + challenge.Id + "\"";
                json += ", \"text\":\"" + challenge.ChallengeText + "\"";
                json += ", \"status\": \"" + challenge.Status + "\"";
                json += ", \"challengedPlayer\": " + challenge.ChallengedPlayer.ToJson();
                json += ", \"challenge\": " + challenge.ChallengeGame.ToJson();
                json += ", \"response\": " + challenge.ResponseGame.ToJson();
                json += "}";
                string req = REST.APIURL + "/challenge";
                string response = REST.put(req, json);
            }
            else
            {
                string json = "{\"id\": \"" + challenge.Id + "\"";
                json += ", \"text\":\"" + challenge.ChallengeText + "\"";
                json += ", \"status\": \"" + challenge.Status + "\"";
                json += ", \"challengedPlayer\": " + challenge.ChallengedPlayer.ToJson();
                json += ", \"challenge\": " + challenge.ChallengeGame.ToJson();
                json += ", \"response\": " + "null";
                json += "}";
                string req = REST.APIURL + "/challenge";
                string response = REST.put(req, json);
            }
            
        }


        public Challenge getChallenge(int challengeId)
        {
            string req = REST.APIURL + "/challenge/" + challengeId;
            string json = REST.get(req);


            return JsonConvert.DeserializeObject<Challenge>(json);
        }
    }
}
