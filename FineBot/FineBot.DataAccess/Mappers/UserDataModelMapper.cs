using System;
using System.Linq;
using FineBot.DataAccess.BaseClasses;
using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers
{
    public class UserDataModelMapper : DataModelMapper<UserDataModel, User>
    {
        private readonly IDataModelMapper<FineDataModel, Fine> fineModelMapper;

        public UserDataModelMapper(IDataModelMapper<FineDataModel, Fine> fineModelMapper)
        {
            this.fineModelMapper = fineModelMapper;
        }

        public override UserDataModel MapToModel(User user)
        {
            var userDataModel = new UserDataModel
                                          {
                                              Id = user.Id,
                                              SlackId = user.SlackId,
                                              EmailAddress = user.EmailAddress,
                                              DisplayName = user.DisplayName,
                                              Image = user.Image,
                                              Fines = user.Fines.Select(x => this.fineModelMapper.MapToModel(x)).ToList(),
                                          };

            return userDataModel;
        }

        public override User MapToDomain(UserDataModel model)
        {
            var userDataModel = new User
            {
                Id = model.Id,
                SlackId = model.SlackId,
                EmailAddress = model.EmailAddress,
                DisplayName = model.DisplayName,
                Image = model.Image,
                Fines = model.Fines.Select(x => this.fineModelMapper.MapToDomain(x)).ToList(),
            };

            return userDataModel;
        }
    }
}