using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FineBot.API.TeamCityApi;
using FineBot.API.TeamCityApi.TeamCityModels;

namespace FineBot.WepApi.Controllers
{
    public class DashboardController : ApiController
    {
        private readonly ITeamCityApi teamCityApi;

        public DashboardController(ITeamCityApi teamCityApi)
        {
            this.teamCityApi = teamCityApi;
        }

        // GET api/<controller>
        public List<ShallowBuildModel> Get()
        {
            return teamCityApi.GetAllBuildsThatFailed();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}