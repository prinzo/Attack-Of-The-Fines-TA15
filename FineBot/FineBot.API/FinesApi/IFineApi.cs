using System;

namespace FineBot.API.FinesApi
{
    public interface IFineApi
    {
        FineModel IssueFine(Guid issuerId, Guid recipientId, Guid seconderId, string reason);
    }
}