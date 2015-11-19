using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.TeamCityApi.TeamCityModels
{
    public class change
    {
        public string id { get; set; }
        public string version { get; set; }
        public string username { get; set; }
        public string date { get; set; }
        public string href { get; set; }
        public string webUrl { get; set; }
    }
}
