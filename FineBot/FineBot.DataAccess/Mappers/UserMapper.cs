using System;
using System.Linq;
using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers
{
    public class UserMapper : IUserDataMapper
    {
        private readonly IFineDataMapper fineMapper;

        public UserMapper(IFineDataMapper fineMapper)
        {
            this.fineMapper = fineMapper;
        }

        public UserDataModel MapToModel(User user)
        {
            var userDataModel = new UserDataModel
                                          {
                                              Id = user.Id,
                                              SlackId = user.SlackId,
                                              EmailAddress = user.EmailAddress,
                                              DisplayName = user.DisplayName,
                                              Image = user.Image,
                                              Fines = user.Fines.Select(x => this.fineMapper.MapToModel(x)).ToList(),
                                          };

            return userDataModel;
        }

        public User MapToDomain(UserDataModel model)
        {
            var userDataModel = new User
            {
                Id = model.Id,
                SlackId = model.SlackId,
                EmailAddress = model.EmailAddress,
                DisplayName = model.DisplayName,
                Image = model.Image,
                Fines = model.Fines.Select(x => this.fineMapper.MapToDomain(x)).ToList(),
            };

            return userDataModel;
        }
    }
}