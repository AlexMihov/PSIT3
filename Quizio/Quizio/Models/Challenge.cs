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
        /// <summary>
        /// Gets or sets the ID of a challange to interact with the db as an integer.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the status of a <c>Challange</c> as a strimg to determine if it's pending, concluded or refused
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets the the ChallangeGame.
        /// </summary>
        public Game ChallengeGame { get; set; }
        /// <summary>
        /// Gets or sets the the ResponseGame
        /// </summary>
        public Game ResponseGame { get; set; }
        /// <summary>
        /// Gets or sets the Player which was challanged, so that it can be displayed and compared
        /// </summary>
        public Player ChallengedPlayer { get; set; }
        /// <summary>
        /// Gets or sets the the Text which is sent to the ChallangedPlayer
        /// </summary>
        public string ChallengeText { get; set; }
        /// <summary>
        /// The constructor which creates a Challange with the provided parameters
        /// </summary>
        /// <param name="id">The ID of the challange in the database</param>
        /// <param name="challenge"></param>
        /// <param name="response"></param>
        /// <param name="challengedPlayer">The player which was challanged to play</param>
        /// <param name="text">The text which is sent to the challanged Player</param>
        /// <param name="status">The current status of the player, can be either pending, concluded or refused</param>
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
