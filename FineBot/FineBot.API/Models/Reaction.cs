using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.Models
{
    public class Reaction
    {
        public string name { get; set; }
        public int count { get; set; }
        public string[] users { get; set; }
    }
}
