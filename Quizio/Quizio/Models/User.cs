using Newtonsoft.Json;

namespace Quizio.Models
{
    /// <summary>
    /// The class user is a model for a player of the game. It saves the id, the name, the status, the location and the email address of the friend.
    /// </summary>
    public class User : Player
    {
        /// <summary>
        /// Gets or sets the location of a user as a string.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the status of a user as a string.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the email address of a user as a string.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Constructor of the user class, it takes an id, a name, a status and location. It can be used to create a user object from a json string.
        /// </summary>
        /// <param name="id">The id a user has in the db as an integer</param>
        /// <param name="name">The name of the user as a string</param>
        /// <param name="status">The name of the user as a string</param>
        /// <param name="location">The name of the user as a string</param>
        /// <param name="email">The email address of the user as a string</param>
        [JsonConstructor]
        public User(int id, string name, string status, string location, string email) : base(id, name)
        {
            this.Status = status;
            this.Location = location;
            this.Email = email;
        }

        internal static string ToJson()
        {
            throw new System.NotImplementedException();
        }
    }
}