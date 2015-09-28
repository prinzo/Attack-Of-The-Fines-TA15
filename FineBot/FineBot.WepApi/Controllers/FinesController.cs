using System;
using System.Collections.Generic;
using System.Web.Http;
using FineBot.API.FinesApi;
using FineBot.API.Mappers;
using FineBot.API.UsersApi;
using FineBot.WepApi.Models;
using FineBot.Common.Infrastructure;

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
        public FeedFineModel IssueFine(NewFineModel newFine)
        {
            FeedFineModel fineModel = this.fineApi.IssueFineFromFeed(new Guid(newFine.IssuerId), new Guid(newFine.RecipientId), newFine.Reason);

            return fineModel;
        }

        [HttpPost]
        public FeedFineModel IssuePayment(NewPaymentModel newPaymentModel) {

            PaymentModel payment = new PaymentModel {
                PayerId = newPaymentModel.PayerId,
                RecipientId = newPaymentModel.RecipientId,
                TotalFinesPaid = newPaymentModel.TotalFinesPaid,
                Image = null
            };

            ValidationResult validationResult;

            FeedFineModel fineModel = this.fineApi.PayFines(payment, out validationResult);

            return fineModel;
        }

        [HttpGet]
        public FeedFineModel[] GetFines() {
            return this.fineApi.GetLatestSetOfFines(0, 10).ToArray();
        }
                
    }
}
