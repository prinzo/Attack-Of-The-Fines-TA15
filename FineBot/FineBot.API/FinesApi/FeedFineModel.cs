using System;
using FineBot.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FineBot.API.FinesApi {
    public class FeedFineModel {
        public Guid Id { get; set; }

        public Guid IssuerId { get; set; }

        public Guid? PayerId { get; set; }

        public string IssuerDisplayName { get; set; }

        public Guid? SeconderId { get; set; }

        public bool Pending { get; set; }

        public string Reason { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverDisplayName { get; set; }

        public string Platform { get; set; }

        public DateTime AwardedDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public byte[] ProfilePicture { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string PaymentImage { get; set; }

        public string UserImage { get; set; }


        public bool IsPaid
        {
            get
            {
                return this.PayerId.HasValue;
            }
        }

        public void BuildNewFineFeedModelFromExistingModel(FeedFineModel feedFineModel)
        {
            this.Id = feedFineModel.Id;
            this.IssuerId = feedFineModel.IssuerId;
            this.Reason = feedFineModel.Reason;
            this.SeconderId = feedFineModel.SeconderId;
            this.Pending = feedFineModel.Pending;
            this.AwardedDate = feedFineModel.AwardedDate;
            this.IssuerDisplayName = feedFineModel.IssuerDisplayName;
            this.ReceiverDisplayName = feedFineModel.ReceiverDisplayName;
            this.PaidDate = null;
            this.PayerId = null;
            this.PaymentImage = feedFineModel.PaymentImage ?? "../../../../content/defaultUser.png"; 
            this.ModifiedDate = feedFineModel.AwardedDate;
            this.UserImage = feedFineModel.UserImage;
        }

    }

}
