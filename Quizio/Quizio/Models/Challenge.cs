using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    /// <summary>
    /// The Challenge Class represents a Challenge, which is sent
    /// to the current User from his Friends.
    /// </summary>
    public class Challenge
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public Game ChallengeGame { get; set; }

        public Game ResponseGame { get; set; }

        public Player ChallengedPlayer { get; set; }

        public string ChallengeText { get; set; }

        [JsonConstructor]
        public Challenge(int id, Game challenge, Game response, Player challengedPlayer, string text, string status)
        {
            this.Id = id;
            this.ChallengeGame = challenge;
            this.ResponseGame = response;
            this.ChallengedPlayer = challengedPlayer;
            this.ChallengeText = text;
            this.Status = status;
        }
    }
}
