﻿using System;
using System.Collections.Generic;
using FineBot.Common.Infrastructure;

namespace FineBot.API.FinesApi
{
    public interface IFineApi
    {
        FineModel IssueFine(Guid issuerId, Guid recipientId, string reason);

        List<FineModel> GetAllPendingFines();

        FineWithUserModel SecondOldestPendingFine(Guid userId);

        FineWithUserModel SecondNewestPendingFine(Guid userId);

        ValidationResult PayFines(Guid userId, Guid payerId, int number, PaymentImageModel paymentImage);

        List<FeedFineModel> GetLatestSetOfFines(int index, int pageSize);

        ValidationResult PayFines(Guid userId, Guid payerId, int number, byte[] image, string mimeType, string fileName);
    }
}