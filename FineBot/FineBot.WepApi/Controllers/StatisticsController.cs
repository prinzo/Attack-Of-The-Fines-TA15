using System;
using System.Collections.Generic;
using System.Web.Http;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;

namespace FineBot.WepApi.Controllers
{
    public class StatisticsController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public StatisticsController(
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
        
        
    }
}
