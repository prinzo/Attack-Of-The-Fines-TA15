using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Castle.Windsor;
using FineBot.API.FinesApi;
using FineBot.API.MemberInfo;
using FineBot.API.UsersApi;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.WepApi.DI;
using Microsoft.Ajax.Utilities;

namespace FineBot.WepApi.Controllers
{
    public class DashboardController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;
        private readonly IMemberInfoApi memberInfoApi;
        private readonly IRepository<User, Guid> userRepository; 

        public DashboardController(IUserApi userApi, 
            IFineApi fineApi,
            IMemberInfoApi memberInfoApi,
            IRepository<User, Guid> userRepository)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
            this.memberInfoApi = memberInfoApi;
            this.userRepository = userRepository;


        }

        public List<UserModel> GetLeaderBoard()
        {
            var list = this.memberInfoApi.GetAllMemberInformation();
            var leaderboardUser = userApi.GetLeaderboard(10);
            return leaderboardUser;
        }

        // GET api/dashboard/5
        public string Get(int id)
        {
            return "value";
        }

        public void IssueFineTester()
        {
            fineApi.IssueFine(new Guid("9f909fc1-405d-43ff-8b9d-381160892c61"), new Guid("e8f891fb-eff6-4787-bd02-636e5edd93b6"), new Guid(), "test");
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
