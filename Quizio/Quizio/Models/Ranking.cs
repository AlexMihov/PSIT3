using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Ranking
    {
        public int Position { get; set; }

        public string PlayerName { get; set; }

        public int Points { get; set; }

        public Ranking(int pos, string name, int points)
        {
            this.Position = pos;
            this.PlayerName = name;
            this.Points = points;
        }
    }
}
