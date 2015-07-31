
namespace FineBot.WepApi.Models {
    public class PaidFineFeedItemModel : FeedItemModel {
        public byte[] Proof { get; set; }

        public int TotalLikes { get; set; }

        public int TotalDislikes { get; set; }

        public bool HasLiked { get; set; }

        public bool HasDisliked { get; set; }
    }
}