using System;
using System.Collections.Generic;
using FineBot.Infrastructure;

namespace FineBot.API.FinesApi
{
    public interface IFineApi
    {
        FineModel IssueFine(Guid issuerId, Guid recipientId, string reason);

        List<FineModel> GetAllPendingFines();

        FineWithUserModel SecondOldestPendingFine(Guid userId);

        FineWithUserModel SecondNewestPendingFine(Guid userId);

        ValidationResult PayFines(Guid userId, int number, byte[] image, string mimeType, string fileName);

        List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize);

        ValidationResult PayFines(Guid userId, int number, PaymentImageModel paymentImage);
    }
}