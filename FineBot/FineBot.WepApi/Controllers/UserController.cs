using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FineBot.API.UsersApi;

namespace FineBot.WepApi.Controllers
{
    [EnableCors("*","*","*")]
    public class UserController : ApiController
    {
        private readonly IUserApi userApi;

        public UserController(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        [HttpGet]
        public UserModel GetUserByEmail(string email)
        {
            return userApi.GetUserByEmail(email);
        }

        [HttpGet]
        public string[] GetUserNameAndSurname(string email)
        {
            return userApi.GetUserNameAndSurnameFromEmail(email);
        }

        [HttpPost]
        public UserModel UpdateUser([FromBody]UserModel userModel)
        {
            return userApi.UpdateUser(userModel);
        }
    }
}
