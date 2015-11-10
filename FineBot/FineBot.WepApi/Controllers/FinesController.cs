using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Castle.Core.Internal;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
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
        public FeedFineModel IssueFine(NewFineModel newFine)
        {
            FeedFineModel fineModel = this.fineApi.IssueFineFromFeed(new Guid(newFine.IssuerId), new Guid(newFine.RecipientId), newFine.Reason);

            return fineModel;
        }

        [HttpPost]
        public PayFineResult IssuePayment(NewPaymentModel newPaymentModel)
        {
            if (newPaymentModel.Image.IsNullOrEmpty())
            {
                return new PayFineResult() {ValidationMessages = new List<ValidationMessage>
                {
                    new ValidationMessage("A payment requires an image", Severity.Error)
                }};    
            }

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

            return result;
        }

        [HttpGet]
        public FeedFineModel[] GetFines() {
            return this.fineApi.GetLatestSetOfFines(0, 10).ToArray();
        }

        [HttpGet]
        public FeedFineModel[] GetNexSetOfFines(int index) {
            return this.fineApi.GetLatestSetOfFines(index, 10).ToArray();
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

        [HttpGet]
        public HttpResponseMessage ExportAllFines()
        {
            HttpResponseMessage result = null;
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(fineApi.ExportAllFines());
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Entelectfines.xlsx"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
