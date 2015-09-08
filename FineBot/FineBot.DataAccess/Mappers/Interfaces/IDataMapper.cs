using FineBot.Interfaces;

namespace FineBot.DataAccess.Mappers.Interfaces
{
    public interface IDataMapper<TData, TDomain>
    {
        TData MapToModel(TDomain entity);

        TDomain MapToDomain(TData model);
    }
}