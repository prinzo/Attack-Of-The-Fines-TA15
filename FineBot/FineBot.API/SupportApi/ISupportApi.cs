using FineBot.Entities;

namespace FineBot.API.SupportApi
{
    public interface ISupportApi
    {
        void AddNewCardToSupport();
        SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel);
    }
}
