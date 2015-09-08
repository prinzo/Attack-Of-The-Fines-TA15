using System;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers.Interfaces
{
    public interface IUserDataMapper : IDataMapper<UserDataModel, User>
    {
    }
}