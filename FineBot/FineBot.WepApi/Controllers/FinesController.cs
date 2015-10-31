﻿using System;
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
        public FeedFineModel IssuePayment(NewPaymentModel newPaymentModel)
        {

            string mimeType = newPaymentModel.Image.Substring(0,
                newPaymentModel.Image.IndexOf(";base64,",
                    StringComparison.Ordinal) + ";base64,".Length
                );

            string image = newPaymentModel.Image.Substring(
                newPaymentModel.Image.IndexOf(";base64,",
                    StringComparison.Ordinal) + ";base64,".Length
                );


            PaymentModel payment = new PaymentModel {
                PayerId = newPaymentModel.PayerId,
                RecipientId = newPaymentModel.RecipientId,
                TotalFinesPaid = newPaymentModel.TotalFinesPaid,
                Image = Convert.FromBase64String(image),
                MimeType = mimeType
            };

            var result = this.fineApi.PayFines(payment);

            //TODO: Come up with a way to handle these errors on the front end, maybe return the result model?
            if(result.HasErrors)
            {
                return new FeedFineModel();
            }

            return result.FeedFineModel;
        }

        [HttpGet]
        public FeedFineModel[] GetFines() {
            return this.fineApi.GetLatestSetOfFines(0, 10).ToArray();
        }

        [HttpGet]
        public byte[] GetImageForId(Guid id) {
            return this.fineApi.GetImageForPaymentId(id);
        }

        [HttpPost]
        public bool SecondFine(FineInteractingModel secondModel) {
            return this.fineApi.SecondFineById(secondModel.FineId, secondModel.UserId);
        }

        [HttpPost]
        public ApprovalResult ApprovePayment(FineInteractingModel interactingModel) {
            return this.fineApi.ApprovePayment(interactingModel.FineId, interactingModel.UserId);
        }

        [HttpPost]
        public ApprovalResult DisapprovePayment(FineInteractingModel interactingModel) {
            return this.fineApi.DisapprovePayment(interactingModel.FineId, interactingModel.UserId);
        }

        [HttpGet]
        public List<UserModel> GetUserApprovedByList(Guid paymentId) {
            return this.fineApi.GetUsersApprovedBy(paymentId);
        }

        [HttpGet]
        public List<UserModel> GetUserDisapprovedByList(Guid paymentId) {
            return this.fineApi.GetUsersDisapprovedBy(paymentId);
        }
    }
}
