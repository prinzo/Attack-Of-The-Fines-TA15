using FineBot.API.Models;

namespace FineBot.API.ChannelApi
{
    public class ChannelListResponseModel
    {
        public bool ok { get; set; }
        public Channel[] channels { get; set; }
        public string error { get; set; }
    }
}
