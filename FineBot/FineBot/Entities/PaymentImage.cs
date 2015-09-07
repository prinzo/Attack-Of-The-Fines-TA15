using FineBot.Abstracts;

namespace FineBot.Entities
{
    public class PaymentImage : GuidIdentifiedEntity
    {
        public PaymentImage(){}

        public PaymentImage(byte[] imageBytes, string mimeType, string fileName)
        {
            this.ImageBytes = imageBytes;
            this.MimeType = mimeType;
            this.FileName = fileName;
        }

        public byte[] ImageBytes { get; set; }

        public string MimeType { get; set; }

        public string FileName { get; set; }
    }
}