﻿using System;
using System.Linq;
using FineBot.API.Extensions;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IFineMapper fineMapper;

        public UserMapper(IFineMapper fineMapper)
        {
            this.fineMapper = fineMapper;
        }

        public UserModel MapToModelShallow(User user)
        {
            if(user == null) return null;

            return new UserModel
                   {
                       Id = user.Id,
                       SlackId = user.SlackId,
                       EmailAddress = user.EmailAddress,
                       DisplayName = user.DisplayName,
                       AwardedFineCount = user.Fines.Count(x => !x.Pending),
                       PendingFineCount = user.Fines.Count(x => x.Pending),
                       Image = user.Image
                   };
        }

        public UserModel MapToModel(User user)
        {
            var userModel = this.MapToModelShallow(user);

            userModel.Fines = user.Fines.Select(x => this.fineMapper.MapToModel(x)).ToList();

            return userModel;
        }

        public UserModel MapToModelWithDate(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                DisplayName = user.DisplayName,
                Image = user.Image,
                AwardedFineCount = user.Fines.Count(x => !x.Pending && x.AwardedDate >= DateTime.Today)
            };
        }

        public UserModel MapToModelForThisWeek(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                DisplayName = user.DisplayName,
                Image = user.Image,
                AwardedFineCount = user.Fines.Count(x => !x.Pending && x.AwardedDate >= DateTime.Now.StartOfWeek())
            };
        }

        public UserModel MapToModelForThisMonth(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                DisplayName = user.DisplayName,
                Image = user.Image,
                AwardedFineCount = user.Fines.Count(x => !x.Pending && x.AwardedDate >= DateTime.Now.StartOfMonth())
            };
        }

        public UserModel MapToModelForThisYear(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                DisplayName = user.DisplayName,
                Image = user.Image,
                AwardedFineCount = user.Fines.Count(x => !x.Pending && x.AwardedDate >= DateTime.Now.StartOfYear())
            };
        }

        public UserModel MapToModelSmall(User user) {
            if (user == null) return null;

            return new UserModel {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Image = user.Image
            };
        }
    }
}