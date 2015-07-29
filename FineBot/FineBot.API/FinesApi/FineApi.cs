using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, Guid> userRepository;
        private readonly IFineMapper fineMapper;
        private readonly IUserMapper userMapper;

        public FineApi(
            IRepository<User, Guid> userRepository,
            IFineMapper fineMapper,
            IUserMapper userMapper
            )
        {
            this.userRepository = userRepository;
            this.fineMapper = fineMapper;
            this.userMapper = userMapper;
        }

        public FineModel IssueFine(Guid issuerId, Guid recipientId, string reason)
        {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason);

            this.userRepository.Save(user);

            return this.fineMapper.MapToModel(fine);
        }

        public List<FineModel> GetAllPendingFines()
        {
            var fines = from user in this.userRepository.FindAll(new UserSpecification().WithPendingFines())
                from fine in user.Fines
                select this.fineMapper.MapToModel(fine);

            return fines.ToList();
        }

        public FineWithUserModel SecondOldestPendingFine(Guid userId)
        {
            var pendingFines = from user in this.userRepository.FindAll(new UserSpecification().WithPendingFines())
                from fine in user.Fines
                select new {user, fine};

            var oldestPendingFine = pendingFines.OrderBy(x => x.fine.AwardedDate).FirstOrDefault();

            if(oldestPendingFine == null) return null;

            Fine fineToBeSeconded = oldestPendingFine.user.Fines.Where(x => x.Pending).OrderBy(x => x.AwardedDate).First();

            fineToBeSeconded.Second(userId);

            this.userRepository.Save(oldestPendingFine.user);

            return this.fineMapper.MapToModelWithUser(fineToBeSeconded, this.userMapper.MapToModelShallow(oldestPendingFine.user));
        }

        public FineWithUserModel SecondNewestPendingFine(Guid userId)
        {
            var pendingFines = from user in this.userRepository.FindAll(new UserSpecification().WithPendingFines())
                from fine in user.Fines
                select new {user, fine};

            var newestPendingFine = pendingFines.OrderByDescending(x => x.fine.AwardedDate).FirstOrDefault();

            if (newestPendingFine == null) return null;

            Fine fineToBeSeconded = newestPendingFine.user.Fines.Where(x => x.Pending).OrderByDescending(x => x.AwardedDate).First();

            fineToBeSeconded.Second(userId);

            this.userRepository.Save(newestPendingFine.user);
            
            return this.fineMapper.MapToModelWithUser(fineToBeSeconded, this.userMapper.MapToModelShallow(newestPendingFine.user));
        }
    }
}