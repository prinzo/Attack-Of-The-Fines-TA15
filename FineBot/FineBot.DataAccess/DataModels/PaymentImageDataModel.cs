using FineBot.Abstracts;

namespace FineBot.DataAccess.DataModels
{
    public class PaymentImageDataModel : GuidIdentifiedEntity
    {
        public byte[] ImageBytes { get; set; }

        public string MimeType { get; set; }

        public string FileName { get; set; }
    }
}