using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Models
{
    public class Notification
    {
        public string Message { get; set; }

        public string Date { get; set; }

        public Notification(string message)
        {
            this.Message = message;
        }

        public Notification(string message, string date)
        {
            this.Message = message;
            this.Date = date;
        }
    }
}
