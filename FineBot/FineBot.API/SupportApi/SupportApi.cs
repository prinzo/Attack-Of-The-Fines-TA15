using System;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Interfaces;
using TrelloNet;

namespace FineBot.API.SupportApi
{
    public class SupportApi : ISupportApi
    {
        private readonly IRepository<SupportTicket, SupportTicketDataModel, Guid> supportTicketRepository;
        private readonly ISupportTicketMapper supportTicketMapper;

        public SupportApi(IRepository<SupportTicket, SupportTicketDataModel, Guid> supportTicketRepository,
            ISupportTicketMapper supportTicketMapper)
        {
            this.supportTicketRepository = supportTicketRepository;
            this.supportTicketMapper = supportTicketMapper;
        }


        public void AddNewCardToSupport()
        {
            ITrello trello = new Trello("f179fdf3799a9e5b7239b88963268f98");
            trello.Authorize("0bc833ffc2b77959f6707d1e6ef56724f76ef748f150836d0d4654feb62c270c");
            var myBoard = trello.Boards.WithId("55b244f75471c89c417c616f");
            var supportList = trello.Lists.ForBoard(myBoard).FirstOrDefault(x => x.Name == "Support");
            var card = new NewCard("Test Card", supportList) {Desc = "This is a test card"};
            trello.Cards.Add(card);
        }

        public SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel)
        {
            var supportTicket = supportTicketMapper.MapToDomain(supportTicketModel);

            supportTicketRepository.Save(supportTicket);

            return supportTicketMapper.MapToModel(supportTicket);
        }
    }
}
