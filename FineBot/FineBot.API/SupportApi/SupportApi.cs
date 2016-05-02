using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.Common.ExtensionMethods;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Enums;
using FineBot.Interfaces;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;

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

            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
            TrelloAuthorization.Default.AppKey = "f179fdf3799a9e5b7239b88963268f98";
            TrelloAuthorization.Default.UserToken = "0bc833ffc2b77959f6707d1e6ef56724f76ef748f150836d0d4654feb62c270c";
        }


        public void AddNewCardToSupport(SupportTicketModel supportTicketModel)
        {
            const string fineAppBoardId = "55b244f75471c89c417c616f";
            var board = new Board(fineAppBoardId);
            var supportList = board.Lists.First(x => x.Name.Equals("Support"));
            var card = supportList.Cards.Add(supportTicketModel.Subject);
            card.Description = string.Format("{0}\n\nStatus: {1}\n\nType: {2}", supportTicketModel.Message,
                ((Status)supportTicketModel.Status).ToDescription(),
                ((SupportType)supportTicketModel.Type).ToDescription());
            
        }

        public SupportTicketModel CreateSupportTicket(SupportTicketModel supportTicketModel)
        {
            var supportTicket = supportTicketMapper.MapToDomain(supportTicketModel);
            supportTicket.CreatedDate = DateTime.UtcNow;
            supportTicket.Status = (int) Status.Open;
            supportTicketRepository.Save(supportTicket);

            return supportTicketMapper.MapToModel(supportTicket);
        }

        public List<SupportTicketModel> GetAllSupportTickets()
        {
            var supportTickets = supportTicketRepository.GetAll().ToList();
            return supportTicketMapper.MapAllToModel(supportTickets);
        }

        public List<SupportTicketModel> GetSupportTicketsForUser(Guid userId)
        {
            var supportTickets = supportTicketRepository.GetAll().Where(x => x.UserId == userId).ToList();
            return supportTicketMapper.MapAllToModel(supportTickets);
        }

        public SupportTicketModel UpdateTicketStatus(SupportTicketModel supportTicketModel)
        {
            var supportTicket = supportTicketRepository.Get(supportTicketModel.Id);
            supportTicket.Status = supportTicketModel.Status;
            supportTicketRepository.Save(supportTicket);

            return supportTicketMapper.MapToModel(supportTicket);
        }

        public SupportTicketModel CreateSupportTicketOnTrello(SupportTicketModel supportTicketModel)
        {
            var model = this.CreateSupportTicket(supportTicketModel);
            this.AddNewCardToSupport(model);

            return model;
        }
    }
}
