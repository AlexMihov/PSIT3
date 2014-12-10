using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Ranking
    {
        /// <summary>
        /// Gets or sets the the position of the player in the ranking
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Gets or sets the The name of the player which is to be displayed
        /// </summary>
        public string PlayerName { get; set; }
        /// <summary>
        /// Gets or sets the the ammount of points a player has accumulated
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// A ranking is an entry in the global highscore which can be seen by players
        /// </summary>
        /// <param name="position">The position which a player currently has</param>
        /// <param name="name">The name of the player which is displayed</param>
        /// <param name="points">The ammount of accumulated points by a player</param>
        [JsonConstructor]
        public Ranking(int position, string name, int points)
        {
            this.Position = position;
            this.PlayerName = name;
            this.Points = points;
        }
    }
}
