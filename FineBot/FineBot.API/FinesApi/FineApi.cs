﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FineBot.Abstracts;
using FineBot.API.Mappers.Interfaces;
using FineBot.Common.Infrastructure;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Infrastructure;
using FineBot.Interfaces;
using FineBot.Specifications;
using FineBot.API.UsersApi;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, UserDataModel, Guid> userRepository;
        private readonly IRepository<Payment, PaymentDataModel, Guid> paymentRepository;
        private readonly IFineMapper fineMapper;
        private readonly IUserMapper userMapper;
        private readonly IPaymentMapper paymentMapper;

        public FineApi(
            IRepository<User, UserDataModel, Guid> userRepository,
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository,
            IFineMapper fineMapper,
            IUserMapper userMapper,
            IPaymentMapper paymentMapper
            )
        {
            this.userRepository = userRepository;
            this.paymentRepository = paymentRepository;
            this.fineMapper = fineMapper;
            this.userMapper = userMapper;
            this.paymentMapper = paymentMapper;
        }

        public FineModel IssueFine(Guid issuerId, Guid recipientId, string reason)
        {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason);

            this.userRepository.Save(user);

            return this.fineMapper.MapToModel(fine);
        }

        public void IssueAutoFine(Guid issuerId, Guid recipientId, Guid seconderId, string reason)
        {
            var user = userRepository.Get(recipientId);
            var fine = user.IssueFine(issuerId, reason);
            userRepository.Save(user);

            var seconder = userRepository.Get(seconderId);
            SecondNewestPendingFine(seconder.Id);
        }

        public FeedFineModel IssueFineFromFeed(Guid issuerId, Guid recipientId, string reason) {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason);

            this.userRepository.Save(user);

            User issuer = this.userRepository.Find(new Specification<User>(x => x.Id == issuerId));
            User recipient = this.userRepository.Find(new Specification<User>(x => x.Id == recipientId));

            return this.fineMapper.MapToFeedModelWithPayment(fine, issuer, recipient, null);
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

        public bool SecondFineById(Guid fineId, Guid userId) {
            var fine = this.userRepository.Find(new UserSpecification().WithFineId(fineId));

            fine.GetFineById(fineId).Second(userId);
            this.userRepository.Save(fine);

            return true;
        }

        public List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize) {
            var newFines = (from user in this.userRepository.GetAll()
                from fine in user.Fines
                select 
                    this.fineMapper.MapToFeedModel(fine, 
                    this.userRepository.Find(new UserSpecification().WithId(fine.IssuerId)),
                    user,
                    fine.SeconderId.HasValue
                            ? this.userRepository.Find(new UserSpecification().WithId(fine.SeconderId.Value))
                            : null
                    ) 
                        
                )
                .OrderByDescending(x => x.ModifiedDate)
                .Skip(index)
                .Take(pageSize);

            var paidFines = (from user in this.userRepository.GetAll()
                from fine in user.Fines
                where fine.PaymentId.HasValue
                select
                    this.fineMapper.MapPaymentToFeedModel(
                        this.paymentRepository.Find(new PaymentSpecification().WithId(fine.PaymentId.Value)),
                        this.userRepository.Find(new UserSpecification().WithId(fine.IssuerId)),
                        user,
                        this.paymentRepository.FindAll(new PaymentSpecification().WithId(fine.PaymentId.Value)).Count()
                        )

                )
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .OrderByDescending(x => x.ModifiedDate)
                .Skip(index)
                .Take(pageSize);

            return paidFines.Union(newFines).OrderByDescending(x => x.ModifiedDate).Take(pageSize).ToList();
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

            result = user.CanPayFines(number);

            if(result.HasErrors) return result;

            payment = this.paymentRepository.Save(payment);

            result = user.PayFines(payment, number);

            this.userRepository.Save(user);

            return result;
        }

        public FeedFineModel PayFines(PaymentModel paymentModel, out ValidationResult validation) {
            var user = this.userRepository.Find(new UserSpecification().WithId(paymentModel.RecipientId));
            var payer = this.userRepository.Find(new UserSpecification().WithId(paymentModel.PayerId));

            Payment payment = new Payment(paymentModel.PayerId, paymentModel.Image, "image/png", null);

            validation = payment.ValidatePaymentForUser(user);

            if (validation != null) {
                return null;
            }

            payment = this.paymentRepository.Save(payment);

            validation = user.PayFines(payment, paymentModel.TotalFinesPaid);

            this.userRepository.Save(user);

            return this.fineMapper.MapPaymentToFeedModel(
                        payment,
                        payer,
                        user,
                        paymentModel.TotalFinesPaid
                        );
        }

        public PaymentModel GetSimplePaymentModelById(Guid paymentModelId)
        {
            var payment = this.paymentRepository.Get(paymentModelId);

            return this.paymentMapper.MapToSimpleModel(payment);
        }

        public byte[] GetImageForPaymentId(Guid id) {
            var payment = this.paymentRepository.Find(new Specification<Payment>(x => x.Id == id));

            return payment.PaymentImage.ImageBytes;

        }

        public bool ApprovePayment(Guid paymentId, Guid userId) {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            payment.LikedBy = payment.LikedBy ?? new List<Guid>();

            var count = payment.LikedBy.Count;

            payment.LikedBy.Add(userId);

            this.paymentRepository.Save(payment);

            return payment.LikedBy.Count > count;
        }

        public bool DisapprovePayment(Guid paymentId, Guid userId) {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            payment.DislikedBy = payment.DislikedBy ?? new List<Guid>();

            var count = payment.DislikedBy.Count;

            payment.DislikedBy.Add(userId);

            this.paymentRepository.Save(payment);

            return payment.DislikedBy.Count > count;
        }

        public ApprovalResult GetUsersApprovedBy(Guid paymentId) {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            List<UserModel> users = new List<UserModel>();
            if (payment.DislikedBy != null) {
                users = this.userRepository.FindAll(new UserSpecification().WithIds(payment.DislikedBy))
                    .Select(x => new UserModel {
                        DisplayName = x.DisplayName
                    }).ToList();
            }

            return new ApprovalResult { users = users };
        }

        public ApprovalResult GetUsersDisapprovedBy(Guid paymentId) {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            List<UserModel> users = new List<UserModel>();
            if(payment.DislikedBy != null) {
                users = this.userRepository.FindAll(new UserSpecification().WithIds(payment.DislikedBy))
                    .Select(x => new UserModel {
                        DisplayName = x.DisplayName
                    }).ToList();
            }

            return new ApprovalResult { users = users };
        }

    }
}