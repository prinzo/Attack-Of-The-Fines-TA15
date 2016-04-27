using FineBot.API.Models;

namespace FineBot.API.ChannelApi
{
    public class ChannelInfoResponseModel
    {
        public bool ok { get; set; }
        public Channel channel { get; set; }
        public string error { get; set; }
    }
}
