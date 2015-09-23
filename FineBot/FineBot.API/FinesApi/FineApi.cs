using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.API.Mappers.Interfaces;
using FineBot.Common.Infrastructure;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Infrastructure;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, UserDataModel, Guid> userRepository;
        private readonly IRepository<Payment, PaymentDataModel, Guid> paymentRepository;
        private readonly IFineMapper fineMapper;
        private readonly IUserMapper userMapper;

        public FineApi(
            IRepository<User, UserDataModel, Guid> userRepository,
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository,
            IFineMapper fineMapper,
            IUserMapper userMapper
            )
        {
            this.userRepository = userRepository;
            this.paymentRepository = paymentRepository;
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

        public FeedFineModel IssueFineFromFeed(Guid issuerId, Guid recipientId, string reason) {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason);

            this.userRepository.Save(user);

            User issuer = this.userRepository.Find(new Specification<User>(x => x.Id == issuerId));
            User recipient = this.userRepository.Find(new Specification<User>(x => x.Id == recipientId));

            return this.fineMapper.MapToFeedModel(fine, issuer, recipient);
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
            var fines = (from user in this.userRepository.GetAll()
                from fine in user.Fines
                select 
                    this.fineMapper.MapToFeedModel(fine, 
                    this.userRepository.Find(new UserSpecification().WithId(fine.IssuerId)),
                    user,
                    fine.PaymentId != null ? this.paymentRepository.Find(new PaymentSpecification().WithId(fine.PaymentId.Value)) : null
                    ) 
                        
                )
                        .OrderByDescending(x => x.ModifiedDate)
                        .Skip(index)
                        .Take(pageSize);

            var paidFines = fines.Where(x => x.PayerId != null);
            var unpaidFines = fines.Where(x => x.PayerId == null).ToList();

            /*
             * This is weird I know, but it's necessary as a paid fine and a new fine can be the same object.
             * We need to both be displayed on the feed if the awarded fine is new enough to make it to the top of the feed.
             * For example I award a fine to Amrit for walking too much now and he pays it in an hour. Both the paid and new
             * fine should still display int the feed.
             */
            foreach(var paidFine in paidFines)
            {
                if(paidFine.AwardedDate > fines.Min(x => x.AwardedDate))
                {
                    FeedFineModel newFine = new FeedFineModel();
                    newFine.BuildNewFineFeedModelFromExistingModel(paidFine);

                    unpaidFines.Add(newFine);
                }
            }

            return paidFines.Union(unpaidFines).OrderByDescending(x => x.ModifiedDate).Take(pageSize).ToList();
        }

        public ValidationResult PayFines(Guid userId, Guid payerId, int number, PaymentImageModel paymentImage)
        {
            return this.PayFines(userId, payerId, number, paymentImage.ImageBytes, paymentImage.MimeType, paymentImage.FileName);
        }

        public ValidationResult PayFines(Guid userId, Guid payerId, int number, byte[] image, string mimeType, string fileName)
        {
            var user = this.userRepository.Get(userId);

            Payment payment = new Payment(payerId, image, mimeType, fileName);

            ValidationResult result = payment.ValidatePaymentForUser(user);

            if(result != null)
            {
                return result;
            }

            payment = this.paymentRepository.Save(payment);

            result = user.PayFines(payment, number);

            this.userRepository.Save(user);

            return result;
        }
    }
}