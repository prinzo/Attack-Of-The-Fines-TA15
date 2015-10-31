using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FineBot.API.LdapApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.WepApi.Models;

namespace FineBot.WepApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly ILdapApi ldapApi;

        public UserController(
            IUserApi userApi,
            ILdapApi ldapApi)
        {
            this.userApi = userApi;
            this.ldapApi = ldapApi;
        }

        [HttpGet]
        public UserModel GetUserByEmail(string email)
        {
            return userApi.GetUserByEmail(email);
        }

        [HttpGet]
        public List<UserModel> GetAllDomainUsers(string domainName, string password)
        {
            return ldapApi.GetAllDomainUsers(domainName, password);
        }

 

        [HttpGet]
        public string[] GetUserNameAndSurname(string email)
        {
            return userApi.GetUserNameAndSurnameFromEmail(email);
        }



        [HttpPost]
        public UserModel UpdateUser([FromBody]UserModel userModel)
        {
            userApi.UpdateUser(userModel);
            return userModel;
        }

        [HttpGet]
        public List<UserModel> GetAllUsers() {
            return userApi.GetAllUsers();

        }

        [HttpPost]
        public UserModel UpdateUserImage([FromBody]UserModel userModel)
        {
            userApi.UpdateUserImage(userModel.Id, userModel.Image);
            return userModel;
        }

        [HttpPost]
        public UserModel AuthenticateUser([FromBody]LoginModel loginModel)
        {
            var ldapUser = ldapApi.AuthenticateAgainstDomain(loginModel.Username, loginModel.Password);
            var slackUser = userApi.GetUserByEmail(ldapUser.EmailAddress);
            return ldapApi.MapSlackModelToLdapModel(ldapUser, slackUser);
        }

        public UserStatisticModel GetStatisticsForUser(Guid id)
        {
            UserStatisticModel model = userApi.GetStatisticsForUser(id);

            return model;
        }
    }
}
