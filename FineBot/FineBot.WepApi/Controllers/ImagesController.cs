using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using FineBot.API.FinesApi;

namespace FineBot.WepApi.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly IFineApi fineApi;

        public ImagesController(
                IFineApi fineApi
            )
        {
            this.fineApi = fineApi;
        }

        [HttpGet]
        public HttpResponseMessage GetFinePaymentImage(Guid id)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);

            var paymentModel = this.fineApi.GetSimplePaymentModelById(id);

            var content = new StreamContent(new MemoryStream(paymentModel.Image));

            content.Headers.ContentType = new MediaTypeHeaderValue(paymentModel.MimeType);

            message.Content = content;

            return message;            
        }
    }
}