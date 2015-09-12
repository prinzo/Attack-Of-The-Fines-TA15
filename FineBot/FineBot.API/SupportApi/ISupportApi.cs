﻿using System;
using System.Collections.Generic;
using FineBot.Entities;
using TrelloNet;

namespace FineBot.API.SupportApi
{
    public interface ISupportApi
    {
        void AddNewCardToSupport(SupportTicketModel supportTicketModel);
        SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel);
        List<SupportTicketModel> GetAllSupportTickets();
        List<SupportTicketModel> GetSupportTicketsForUser(Guid userId);
        SupportTicketModel UpdateTicketStatus(SupportTicketModel supportTicketModel);
    }
}
