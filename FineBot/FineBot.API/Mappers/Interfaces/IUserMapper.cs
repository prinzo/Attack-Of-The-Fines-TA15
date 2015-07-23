using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IUserMapper
    {
        UserModel MapToModel(User user);
    }
}