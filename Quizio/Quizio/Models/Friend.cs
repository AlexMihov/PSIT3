using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Friend
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public Friend(string name, string status)
        {
            this.Name = name;
            this.Status = status;
        }

    }
}
