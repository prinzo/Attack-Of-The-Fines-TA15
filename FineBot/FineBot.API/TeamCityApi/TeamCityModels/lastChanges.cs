using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.TeamCityApi.TeamCityModels
{
    public class lastChanges
    {
        public string count { get; set; }
        public List<change> change { get; set; }
    }
}
