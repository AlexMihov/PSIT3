using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    /// <summary>
    /// This class serializes a game played. A game contains an id, the user which played the game,
    /// the time spend playing the game, the given answers in rounds and the quiz with the questions
    /// which have been answers.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets or sets an unique id that belongs to a specific game played.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user, which played the game.
        /// </summary>
        public Friend Player { get; set; }

        /// <summary>
        /// Gets or sets the time it took to finish the game in seconds.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Gets or sets a <c>List</c> of <c>Rounds</c> played in the game.
        /// </summary>
        public List<Round> Rounds { get; set; }

        /// <summary>
        /// Gets or sets the played quiz. It contains all the quiz information and all the 
        /// questions played in this game.
        /// </summary>
        public Quiz PlayedQuiz { get; set; }

        /// <summary>
        /// Gets or sets the category to which the quiz belongs.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Constructs a game object that serializes the data of a quiz played by a user.
        /// This constructor can be used to generate a game object from json.
        /// </summary>
        /// <param name="id">A unique id as a integer.</param>
        /// <param name="user">The player that played the game as user object</param>
        /// <param name="time">The time it took to finish the game in seconds as integer</param>
        /// <param name="rounds">A <c>List</c> of every <c>Round</c> played in the game</param>
        /// <param name="quiz">A the quiz played in this game.</param>
        /// <param name="category">The category to which the quiz belongs.</param>
        [JsonConstructor]
        public Game(int id, Friend player, int time, List<Round> rounds, Quiz quiz, Category category)
        {
            Id = id;
            Player = player;
            Time = time;
            Rounds = rounds;
            PlayedQuiz = quiz;
            Category = category;
        }

        /// <summary>
        /// Constructor used to save a new game. The game id is automaticly set to zero.
        /// Constructs a game object that serializes the data of a quiz played by a user.
        /// </summary>
        /// <param name="user">The player that played the game as user object</param>
        /// <param name="time">The time it took to finish the game in seconds as integer</param>
        /// <param name="rounds">A <c>List</c> of every <c>Round</c> played in the game</param>
        /// <param name="quiz">A the quiz played in this game.</param>
        /// <param name="category">The category to which the quiz belongs.</param>
        public Game(Friend player, int time, List<Round> rounds, Quiz quiz, Category category) : this(0, player, time, rounds, quiz, category) { }

        /// <summary>
        /// Converts the game to a json string.
        /// </summary>
        /// <returns>The game object as a json string.</returns>
        public string ToJson()
        {
            string json = "{ ";
            if (Id !=0)
            {
                  json += "\"id\": \""+ Id + "\", ";
            }
            if (Player != null)
            {
                json += "\"playerId\": " + Player.Id + ", ";
            }
                json += "\"time\": \"" + Time + "\"";
            if (PlayedQuiz != null)
            {
                json += ", \"quizId\": " + PlayedQuiz.Id;
            }
            if (Rounds != null)
            {
                json += ", \"givenanswers\": [" + roundsToJson() + "]";
            }  
            json += "}";
            return json;
        }

        private string roundsToJson()
        {
            string toJson = "";
            IEnumerator<Round> it = Rounds.GetEnumerator();
            if (it.MoveNext())
            {
                toJson += it.Current.ToJson();
            }
            
            while (it.MoveNext())
            {
                toJson += ", " + it.Current.ToJson();
            }
            return toJson;
        }

    }
}
