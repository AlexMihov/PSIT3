using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Player
    {
        /// <summary>
        /// Gets or sets the id of a player as an integer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of a player as a string.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor of the player class, it takes an id, a name, a status and location. It can be used to create a player object from a json string.
        /// </summary>
        /// <param name="id">The id a player has in the db as an integer</param>
        /// <param name="name">The name of the player as a string</param>
        [JsonConstructor]
        public Player(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
