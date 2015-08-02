using System;
using System.Collections.Generic;
using System.Web.Http;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.WepApi.Models;

namespace FineBot.WepApi.Controllers
{
    public class FinesController : ApiController
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public FinesController(
            IUserApi userApi, 
            IFineApi fineApi
            )
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        [HttpPost]
        public bool IssueFine(NewFineModel newFine)
        {
            //FineModel fineModel = fineApi.IssueFine(new Guid(newFine.IssuerId), new Guid(newFine.RecipientId), newFine.Reason);

           // return fineModel != null;

            //todo figure out why json isn't being passed
            //return true;
        }

        [HttpGet]
        public bool Hello() {
           
            return true;
        }
        
    }
}
