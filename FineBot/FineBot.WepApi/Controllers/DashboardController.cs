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
            var leaderboardUser = userApi.GetLeaderboardForThisYear(10);
            return leaderboardUser;
        }

        [HttpGet]
        public List<UserModel> GetLeaderboardToday()
        {
            return userApi.GetLeaderboardToday(10);
        }

        [HttpGet]
        public List<UserModel> GetLeaderboardForWeek()
        {
            return userApi.GetLeaderboardForThisWeek(10);
        }

        [HttpGet]
        public List<UserModel> GetLeaderboardForMonth()
        {
            return userApi.GetLeaderboardForThisMonth(10);
        }
        
        [HttpGet]
        public List<UserModel> GetUsersWithPendingFines()
        {
            return userApi.GetUsersWithPendingFines();
        }

     

        public void IssueFineTester()
        {
            fineApi.IssueFine(new Guid("9f909fc1-405d-43ff-8b9d-381160892c61"), new Guid("e8f891fb-eff6-4787-bd02-636e5edd93b6"), "test");
        }

        
    }
}
