using System;
using FineBot.API.Mappers.Interfaces;
using FineBot.Entities;
using FineBot.Interfaces;

namespace FineBot.API.FinesApi
{
    public class FineApi : IFineApi
    {
        private readonly IRepository<User, Guid> userRepository;
        private readonly IFineMapper fineMapper;

        public FineApi(
            IRepository<User, Guid> userRepository,
            IFineMapper fineMapper
            )
        {
            this.userRepository = userRepository;
            this.fineMapper = fineMapper;
        }

        public FineModel IssueFine(Guid issuerId, Guid recipientId, Guid seconderId, string reason)
        {
            var user = this.userRepository.Get(recipientId);

            var fine = user.IssueFine(issuerId, reason, seconderId);

            return this.fineMapper.MapToModel(fine);
        }
    }
}