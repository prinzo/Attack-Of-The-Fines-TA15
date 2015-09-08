using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FineBot.API.LdapApi;
using FineBot.API.TrelloApi;
using FineBot.API.UsersApi;

namespace FineBot.WepApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly ILdapApi ldapApi;
        private readonly ITrelloApi trelloApi;

        public UserController(
            IUserApi userApi,
            ILdapApi ldapApi,
            ITrelloApi trelloApi)
        {
            this.userApi = userApi;
            this.ldapApi = ldapApi;
            this.trelloApi = trelloApi;
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
        public UserModel AuthenticateUser(string domainName, string password)
        {
            var ldapUser = ldapApi.AuthenticateAgainstDomain(domainName, password);
            var slackUser = userApi.GetUserByEmail(ldapUser.EmailAddress);
            return ldapApi.MapSlackModelToLdapModel(ldapUser, slackUser);
        }

        [HttpGet]
        public string[] GetUserNameAndSurname(string email)
        {
            return userApi.GetUserNameAndSurnameFromEmail(email);
        }

        [HttpPost]
        public string AddTrelloCard()
        {
            try
            {
                trelloApi.AddNewCardToSupport();
            }
            catch (Exception ex){
                
                throw new Exception(ex.Message);
            }

            return "Support Card Successfully Added";
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

    }
}
