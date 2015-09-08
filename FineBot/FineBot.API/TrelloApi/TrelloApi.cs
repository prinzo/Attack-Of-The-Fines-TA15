using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;

namespace FineBot.API.TrelloApi
{
    public class TrelloApi : ITrelloApi
    {
        public void AddNewCardToSupport()
        {
            ITrello trello = new Trello("f179fdf3799a9e5b7239b88963268f98");
            trello.Authorize("0bc833ffc2b77959f6707d1e6ef56724f76ef748f150836d0d4654feb62c270c");
            var myBoard = trello.Boards.WithId("55b244f75471c89c417c616f");
            var supportList = trello.Lists.ForBoard(myBoard).FirstOrDefault(x => x.Name == "Support");
            var card = new NewCard("Test Card", supportList) {Desc = "This is a test card"};
            trello.Cards.Add(card);
        }
    }
}
