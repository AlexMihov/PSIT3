using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
        public string Location { get; set; }

        public Friend(int id, string name, string status, string location)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
            this.Location = location;
        }


        public Friend(string name, string status): this(0, name, status, "") { }


    }
}
