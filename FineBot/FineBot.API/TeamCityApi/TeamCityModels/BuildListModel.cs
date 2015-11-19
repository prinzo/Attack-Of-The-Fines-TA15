using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.TeamCityApi.TeamCityModels
{
    public class BuildListModel
    {
        public string count { get; set; }
        public string href { get; set; }
        public List<ShallowBuildModel> build { get; set; } 
    }
}
