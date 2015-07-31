using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineBot.WepApi.Models {
    public class FeedItemModel {
        
        public string Username { get; set; }
        
        public string UserGuid { get; set; }

        public string Platform { get; set; }

        public string IssuerName { get; set; }

        public string IssuerGuid { get; set; }

    }
}