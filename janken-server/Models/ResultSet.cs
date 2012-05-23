using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Progressive.JankenServer.Models
{
    public class ResultSet
    {
        public int Client0Hand { get; set; }
        public int Client1Hand { get; set; }
        public string Client0Comment { get; set; }
        public string Client1Comment { get; set; }
    }
}
