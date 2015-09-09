using System;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Enums;
using FineBot.ExtensionMethods;
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


        public void AddNewCardToSupport(SupportTicketModel supportTicketModel)
        {
            ITrello trello = new Trello("f179fdf3799a9e5b7239b88963268f98");
            trello.Authorize("0bc833ffc2b77959f6707d1e6ef56724f76ef748f150836d0d4654feb62c270c");
            var myBoard = trello.Boards.WithId("55b244f75471c89c417c616f");
            var supportList = trello.Lists.ForBoard(myBoard).FirstOrDefault(x => x.Name == "Support");
            var card = new NewCard(supportTicketModel.Subject, supportList) 
            {
                Desc = supportTicketModel.Message + Environment.NewLine + 
                "Status: " + ((Status)supportTicketModel.Status).ToDescription() + Environment.NewLine + 
                "Type: " + ((SupportType)supportTicketModel.Type).ToDescription()
            };
            trello.Cards.Add(card);
        }

        public SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel)
        {
            var supportTicket = supportTicketMapper.MapToDomain(supportTicketModel);
            supportTicket.Status = (int) Status.Open;
            supportTicketRepository.Save(supportTicket);

            return supportTicketMapper.MapToModel(supportTicket);
        }
    }
}
