using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, UserDataModel, Guid> userRepository;
        private readonly IFineMapper fineMapper;
        private readonly IUserMapper userMapper;

        public FineApi(
            IRepository<User, UserDataModel, Guid> userRepository,
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

            return Enumerable.ToList(fines);
        }

        public FineWithUserModel SecondOldestPendingFine(Guid userId)
        {
            var pendingFines = this.userRepository.FindAll(new UserSpecification().WithPendingFines());

            var userWithOldestPendingFine = pendingFines.OrderBy(x => x.GetOldestPendingFine().AwardedDate).FirstOrDefault();
            if(userWithOldestPendingFine == null) return null;

            Fine fineToBeSeconded = userWithOldestPendingFine.GetOldestPendingFine();
            fineToBeSeconded.Second(userId);
            this.userRepository.Save(userWithOldestPendingFine);

            return this.fineMapper.MapToModelWithUser(fineToBeSeconded, this.userMapper.MapToModelShallow(userWithOldestPendingFine));
        }

        public FineWithUserModel SecondNewestPendingFine(Guid userId)
        {
            var pendingFines = this.userRepository.FindAll(new UserSpecification().WithPendingFines());

            var userWithNewestPendingFine = pendingFines.OrderByDescending(x => x.GetNewestPendingFine().AwardedDate).FirstOrDefault();
            if (userWithNewestPendingFine == null) return null;

            var fineToBeSeconded = userWithNewestPendingFine.GetNewestPendingFine();
            fineToBeSeconded.Second(userId);
            this.userRepository.Save(userWithNewestPendingFine);
            
            return this.fineMapper.MapToModelWithUser(fineToBeSeconded, this.userMapper.MapToModelShallow(userWithNewestPendingFine));
        }

        public List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize) {
            var fines = Enumerable.Skip((from user in this.userRepository.GetAll()//.OrderByDesc(x => x.AwardedDate)
                            from fine in user.Fines
                            select this.fineMapper.MapToFeedModel(fine, 
                                this.userRepository.Find(new UserSpecification().WithId(fine.IssuerId)),
                                user)), index).Take(pageSize);

            return fines.ToList();
        }

        public void PayFines(Guid userId, int number, PaymentImageModel paymentImage)
        {
            this.PayFines(userId, number, paymentImage.ImageBytes, paymentImage.MimeType, paymentImage.FileName);
        }

        public void PayFines(Guid userId, int number, byte[] image, string mimeType, string fileName)
        {
            var user = this.userRepository.Get(userId);

            user.PayFines(number, image, mimeType, fileName);

            this.userRepository.Save(user);
        }
    }
}