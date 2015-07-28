using System;
using System.Collections.Generic;
using System.Web.Http;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;

namespace FineBot.WepApi.Controllers
{
    public class DashboardController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public DashboardController(
            IUserApi userApi, 
            IFineApi fineApi
            )
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        [HttpGet]
        public List<UserModel> GetLeaderBoard()
        {
            var leaderboardUser = userApi.GetLeaderboard(10);
            return leaderboardUser;
        }

        [HttpGet]
        public List<UserModel> GetLeaderboardToday()
        {
            return userApi.GetLeaderboardToday(10);
        }

        // GET api/dashboard/5
        public string Get(int id)
        {
            return "value";
        }

        public void IssueFineTester()
        {
            fineApi.IssueFine(new Guid("9f909fc1-405d-43ff-8b9d-381160892c61"), new Guid("e8f891fb-eff6-4787-bd02-636e5edd93b6"), "test");
        }

        // PUT api/dashboard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dashboard/5
        public void Delete(int id)
        {
        }
    }
}
