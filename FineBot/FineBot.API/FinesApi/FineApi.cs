using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Castle.Core.Internal;
using FineBot.Abstracts;
using FineBot.API.InfrastructureServices;
using FineBot.API.Mappers.Interfaces;
using FineBot.Common.Infrastructure;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Infrastructure;
using FineBot.Interfaces;
using FineBot.Specifications;
using FineBot.API.UsersApi;
using FineBot.Common.Enums;
using FineBot.Enums;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, UserDataModel, Guid> userRepository;
        private readonly IRepository<Payment, PaymentDataModel, Guid> paymentRepository;
        private readonly IFineMapper fineMapper;
        private readonly IUserMapper userMapper;
        private readonly IPaymentMapper paymentMapper;
        private readonly IExcelExportService<FineExportModel> excelExportService;

        public FineApi(
            IRepository<User, UserDataModel, Guid> userRepository,
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository,
            IFineMapper fineMapper,
            IUserMapper userMapper,
            IPaymentMapper paymentMapper,
            IExcelExportService<FineExportModel> excelExportService)
        {
            this.userRepository = userRepository;
            this.paymentRepository = paymentRepository;
            this.fineMapper = fineMapper;
            this.userMapper = userMapper;
            this.paymentMapper = paymentMapper;
            this.excelExportService = excelExportService;
        }

        public IssueFineResult IssueFine(Guid issuerId, Guid recipientId, string reason)
        {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason, PlatformType.Slack);

            ValidationResult validationResult = this.ValidateFine(fine);
            if (validationResult.HasErrors)
            {
                return new IssueFineResult(validationResult);
            }

            this.userRepository.Save(user);

            return new IssueFineResult();
        }

        public void IssueAutoFine(Guid issuerId, Guid recipientId, Guid seconderId, string reason)
        {
            var user = userRepository.Get(recipientId);
            var fine = user.IssueFine(issuerId, reason, PlatformType.Slack);

            userRepository.Save(user);

            var seconder = userRepository.Get(seconderId);
            SecondNewestPendingFine(seconder.Id);
        }

        public IssueFineResult IssueFineFromFeed(Guid issuerId, Guid recipientId, string reason)
        {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason, PlatformType.WebFrontEnd);

            ValidationResult validationResult = this.ValidateFine(fine);
            if (validationResult.HasErrors) {
                return new IssueFineResult(validationResult);
            }

            this.userRepository.Save(user);

            User issuer = this.userRepository.Find(new Specification<User>(x => x.Id == issuerId));
            User recipient = this.userRepository.Find(new Specification<User>(x => x.Id == recipientId));


            IssueFineResult result = new IssueFineResult
            {
                Fine = this.fineMapper.MapToFeedModelWithPayment(fine, issuer, recipient, null)
            };

            return result;
        }

        public List<FineModel> GetAllPendingFines()
        {
            var fines = from user in this.userRepository.FindAll(new UserSpecification().WithPendingFines())
                        from fine in user.Fines
                        select this.fineMapper.MapToModel(fine);

            return Enumerable.ToList(fines);
        }

        public FineSecondedResult SecondOldestPendingFine(Guid userId)
        {
            var pendingFines = this.userRepository.FindAll(new UserSpecification().WithPendingFines());

            var userWithOldestPendingFine = pendingFines.OrderBy(x => x.GetOldestPendingFine().AwardedDate).FirstOrDefault();

            if (userWithOldestPendingFine == null) return new FineSecondedResult(new ValidationResult().AddMessage(Severity.Error, "Sorry, there are no pending fines to second"));

            Fine fineToBeSeconded = userWithOldestPendingFine.GetOldestPendingFine();

            var result = new FineSecondedResult(fineToBeSeconded.Second(userId));

            if (result.HasErrors) return result;

            this.userRepository.Save(userWithOldestPendingFine);

            result.FineWithUserModel = this.fineMapper.MapToModelWithUser(fineToBeSeconded,
                this.userMapper.MapToModelShallow(userWithOldestPendingFine));

            return result;
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

        public bool SecondFineById(Guid fineId, Guid userId)
        {
            var fine = this.userRepository.Find(new UserSpecification().WithFineId(fineId));

            var result = fine.GetFineById(fineId).Second(userId);

            //TODO: Accommodate returning errors to the front end
            if (result.HasErrors) return false;

            this.userRepository.Save(fine);

            return true;
        }

        public List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize)
        {
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
                let paymentId = fine.PaymentId
                where paymentId != null
                where paymentId.HasValue
                             select
                                 this.fineMapper.MapPaymentToFeedModel(
                                     this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId.Value)),
                                     this.userRepository.Find(new UserSpecification().WithId(fine.IssuerId)),
                                     user,
                                     this.paymentRepository.FindAll(new PaymentSpecification().WithId(paymentId.Value)).Count()
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
            payment.Platform = PlatformType.Slack;

            var result = user.PayFines(payment, number);

            if (result.HasErrors) return result;

            this.paymentRepository.Save(payment);

            this.userRepository.Save(user);

            return result;
        }

        public PayFineResult PayFines(PaymentModel paymentModel)
        {

            PayFineResult payFineResult = this.PerformInitialValidation(paymentModel);

            if (paymentModel == null)
            {
                return payFineResult;
            }

            var user = this.userRepository.Find(new UserSpecification().WithId(paymentModel.RecipientId));
            var payer = this.userRepository.Find(new UserSpecification().WithId(paymentModel.PayerId));

            Payment payment = new Payment(paymentModel.PayerId, paymentModel.Image, "image/png", null);
            payment.Platform = PlatformType.WebFrontEnd;

            var validation = user.PayFines(payment, paymentModel.TotalFinesPaid);
            payFineResult = new PayFineResult(validation);

            if (validation.HasErrors)
            {
                return payFineResult;
            }

            payment = this.paymentRepository.Save(payment);

            this.userRepository.Save(user);

            payFineResult.FeedFineModel = this.fineMapper.MapPaymentToFeedModel(
                payment,
                payer,
                user,
                paymentModel.TotalFinesPaid
                );

            return payFineResult;
        }

        private PayFineResult PerformInitialValidation(PaymentModel paymentModel)
        {
            if (paymentModel.Image.IsNullOrEmpty())
            {
                return new PayFineResult()
                {
                    ValidationMessages = new List<ValidationMessage> { new ValidationMessage("A payment requires an image", Severity.Error) }
                };
            }

            if (paymentModel.TotalFinesPaid < 1)
            {
                return new PayFineResult()
                {
                    ValidationMessages = new List<ValidationMessage>
                    {
                        new ValidationMessage("A payment requires a total number of fines to be paid", Severity.Error)
                    }
                };
            }

            return null;
        }

        public PaymentModel GetSimplePaymentModelById(Guid paymentModelId)
        {
            var payment = this.paymentRepository.Get(paymentModelId);

            return this.paymentMapper.MapToSimpleModel(payment);
        }

        public byte[] GetImageForPaymentId(Guid id)
        {
            var payment = this.paymentRepository.Find(new Specification<Payment>(x => x.Id == id));

            return payment.PaymentImage.ImageBytes;

        }

        public ApprovalResult ApprovePayment(Guid paymentId, Guid userId)
        {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            payment.LikedBy = payment.LikedBy ?? new List<Guid>();

            var count = payment.LikedBy.Count;
            bool? result;

            if (payment.LikedBy.Count(x => x == userId) == 0)
            {
                payment.LikedBy.Add(userId);
                result = true;
            }
            else
            {
                payment.LikedBy.Remove(userId);
                result = false;
            }

            this.paymentRepository.Save(payment);

            return new ApprovalResult { Success = result };
        }

        public ApprovalResult DisapprovePayment(Guid paymentId, Guid userId)
        {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            payment.DislikedBy = payment.DislikedBy ?? new List<Guid>();

            var count = payment.DislikedBy.Count;
            bool? result;

            if (payment.DislikedBy.Count(x => x == userId) == 0)
            {
                payment.DislikedBy.Add(userId);
                result = true;
            }
            else
            {
                payment.DislikedBy.Remove(userId);
                result = false;
            }

            this.paymentRepository.Save(payment);

            return new ApprovalResult { Success = result };
        }

        public List<UserModel> GetUsersApprovedBy(Guid paymentId)
        {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            List<UserModel> users = new List<UserModel>();
            if (payment.LikedBy != null)
            {
                users = this.userRepository.FindAll(new UserSpecification().WithIds(payment.LikedBy))
                    .Select(x => new UserModel
                    {
                        Id = x.Id,
                        DisplayName = x.DisplayName
                    }).ToList();
            }

            return users = users;
        }

        public List<UserModel> GetUsersDisapprovedBy(Guid paymentId)
        {
            var payment = this.paymentRepository.Find(new PaymentSpecification().WithId(paymentId));

            List<UserModel> users = new List<UserModel>();
            if (payment.DislikedBy != null)
            {
                users = this.userRepository.FindAll(new UserSpecification().WithIds(payment.DislikedBy))
                    .Select(x => new UserModel
                    {
                        Id = x.Id,
                        DisplayName = x.DisplayName
                    }).ToList();
            }

            return users = users;
        }

        public int CountAllFinesSuccessfullyIssued()
        {
            return userRepository.GetAll().Sum(x => x.Fines.Count(y => !y.Pending));
        }

        public byte[] ExportAllFines()
        {

            var userFines = from u in userRepository.GetAll()
                            select new FineExportModel()
                            {
                                Email = u.EmailAddress,
                                DisplayName = u.DisplayName,
                                Fines = u.Fines.Count(x => !x.Pending)
                            };

            return excelExportService.WriteObjectData(userFines.OrderByDescending(x=>x.Fines).ToList(), "Entelect Fines");
        }

        private ValidationResult ValidateFine(Fine fine)
        {
            ValidationResult validationResult = new ValidationResult();

            if (fine.Reason.IsNullOrEmpty())
            {
                validationResult.AddMessage(Severity.Error, "A fine requires a reason");
            }

            int userFineAwardedCountForToday =  this.userRepository.FindAll(new UserSpecification().WithFinesAwardedTodayBy(fine.IssuerId)).Count();

            if (userFineAwardedCountForToday >= 2)
            {
                validationResult.AddMessage(Severity.Error, "Only 2 fines per user per day can be awarded");
            }

            return validationResult;
        }
    }
}