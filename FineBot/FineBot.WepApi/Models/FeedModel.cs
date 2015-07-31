using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineBot.WepApi.Models {
    public class FeedModel {
        public List<FeedItemModel> FeedItems { get; set; }

        public int PageNumber { get; set; }

        public int TotalFeedItemsOnPage { get; set; }
    }
}