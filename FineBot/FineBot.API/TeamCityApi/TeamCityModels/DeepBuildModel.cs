using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.TeamCityApi.TeamCityModels
{
    public class DeepBuildModel
    {
        public string id { get; set; }
        public string number { get; set; }
        public string queuedDate { get; set; }
        public string startDate { get; set; }
        public string finishDate { get; set; }
        public string status { get; set; }
        public lastChanges lastChanges { get; set; }
    }
}
