
namespace FineBot.WepApi.Models {
    public class NewFineFeedItemModel : FeedItemModel {
        public string Reason { get; set; }

        public int SecondedTotalTimes { get; set; }

        public bool HasSeconded { get; set; }
    }
}