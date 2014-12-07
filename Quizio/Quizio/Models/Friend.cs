using Newtonsoft.Json;

namespace Quizio.Models
{
    /// <summary>
    /// The class friend is a model for the friends of a player of the quiz. It saves the id, the name, the status, and the location of the friend.
    /// </summary>
    public class Friend : Player
    {
        /// <summary>
        /// Gets or sets the status of a friend as a string.
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Gets or sets the location of a friend as a string.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Constructor of the friend class, it takes an id, a name, a status and location. It can be used to create a friend object from a json string.
        /// </summary>
        /// <param name="id">The id a friend has in the db as an integer</param>
        /// <param name="name">The name of the friend as a string</param>
        /// <param name="status">The status of the friend as a string</param>
        /// <param name="location">The location of the friend as a string</param>
        [JsonConstructor]
        public Friend(int id, string name, string status, string location) : base(id, name)
        {
            this.Status = status;
            this.Location = location;
        }
    }
}