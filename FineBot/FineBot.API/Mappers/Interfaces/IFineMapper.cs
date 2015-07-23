using FineBot.API.FinesApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IFineMapper
    {
        FineModel MapToModel(Fine fine);
    }
}