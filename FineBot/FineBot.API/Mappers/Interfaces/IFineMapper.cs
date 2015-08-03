using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IFineMapper
    {
        FineModel MapToModel(Fine fine);

        FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel);

        FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver);
    }
}