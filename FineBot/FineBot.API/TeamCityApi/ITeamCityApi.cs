using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.TeamCityApi.TeamCityModels;

namespace FineBot.API.TeamCityApi
{
    public interface ITeamCityApi
    {
        DeepBuildModel GetBuildById(string buildId);
        List<ShallowBuildModel> GetAllBuildsThatFailed();
    }
}
