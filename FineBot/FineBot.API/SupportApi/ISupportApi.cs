using FineBot.Entities;

namespace FineBot.API.SupportApi
{
    public interface ISupportApi
    {
        void AddNewCardToSupport(SupportTicketModel supportTicketModel);
        SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel);
    }
}
