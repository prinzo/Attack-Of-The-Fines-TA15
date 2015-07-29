using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FineBot.API.UsersApi;

namespace FineBot.WepApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserApi userApi;

        public UserController(IUserApi userApi)
        {
            this.userApi = userApi;
        }


        public UserModel GetUserByEmail(string email)
        {
            return userApi.GetUserByEmail(email);
        }

        [HttpPost]
        public UserModel UpdateUser([FromBody] UserModel userModel)
        {
            return userApi.UpdateUser(userModel);
        }
    }
}
