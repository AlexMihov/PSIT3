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

        public Quiz Quiz { get; set; }

        //public Game ChallengeGame { get; set; }

        //public Game ResponseGame { get; set; }

        public Friend From { get; set; }

        public List<Round> ChallengerData { get; set; }

        public List<Round> MyData { get; set; }

        public int EnemyTime { get; set; }

        public string CategoryName { get; set; }

        public string ChallengeText { get; set; }

        [JsonConstructor]
        public Challenge(int id, Friend from, Quiz quiz, List<Round> myData, List<Round> challengerData, int enemytime, string categoryName, string text)
        {
            this.Id = id;
            this.From = from;
            this.Quiz = quiz;
            this.ChallengerData = challengerData;
            this.MyData = myData;
            this.EnemyTime = enemytime;
            this.CategoryName = categoryName;
            this.ChallengeText = text;
        }
    }
}
