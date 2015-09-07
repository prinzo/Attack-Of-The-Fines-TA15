using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.FinesApi {
    public class FeedFineModel {
        public Guid IssuerId { get; set; }

        public string IssuerDisplayName { get; set; }

        public Guid? SeconderId { get; set; }

        public bool Pending { get; set; }

        public string Reason { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverDisplayName { get; set; }

        public string Platform { get; set; }

        public DateTime AwardedDate { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
