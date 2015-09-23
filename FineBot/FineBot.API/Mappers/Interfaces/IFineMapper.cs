using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IFineMapper
    {
        FineModel MapToModel(Fine fine, Payment payment);

        FineModel MapToModel(Fine fine);

        FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel, Payment payment);

        FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel);

        FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver, Payment payment);

        FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver);
    }
}