using System;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IUserMapper
    {
        UserModel MapToModelShallow(User user);
        UserModel MapToModelWithDate(User user, DateTime date);

        UserModel MapToModel(User user);
        UserModel MapToModelSmall(User user);
    }
}