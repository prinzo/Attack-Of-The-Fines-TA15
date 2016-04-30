using System;
using System.Collections.Generic;
using FineBot.Common.Infrastructure;
using FineBot.API.UsersApi;
using FineBot.Enums;

namespace FineBot.API.FinesApi
{
    public interface IFineApi
    {
        IssueFineResult IssueFine(Guid issuerId, Guid recipientId, string reason);

        void IssueFinesFromReactions(DateTime startDateTime, ChatRoomType chatRoomType);

        IssueFineResult IssueFineFromFeed(Guid issuerId, Guid recipientId, string reason);

        List<FineModel> GetAllPendingFines();

        FineSecondedResult SecondOldestPendingFine(Guid userId);

        FineWithUserModel SecondNewestPendingFine(Guid userId);

        ValidationResult PayFines(Guid userId, Guid payerId, int number, PaymentImageModel paymentImage);

        List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize);

        ValidationResult PayFines(Guid userId, Guid payerId, int number, byte[] image, string mimeType, string fileName);

        PayFineResult PayFines(PaymentModel paymentModel);

        PaymentModel GetSimplePaymentModelById(Guid paymentModelId);

        void IssueAutoFine(Guid issuerId, Guid recipientId, Guid seconderId, string reason);

        byte[] GetImageForPaymentId(Guid id);

        bool SecondFineById(Guid id, Guid userId);

        ApprovalResult DisapprovePayment(Guid paymentId, Guid userId);

        ApprovalResult ApprovePayment(Guid paymentId, Guid userId);

        List<UserModel> GetUsersApprovedBy(Guid paymentId);

        List<UserModel> GetUsersDisapprovedBy(Guid paymentId);

        int CountAllFinesSuccessfullyIssued();

        byte[] ExportAllFines();
    }
}