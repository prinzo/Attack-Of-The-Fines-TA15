using FineBot.Interfaces;

namespace FineBot.DataAccess.Mappers.Interfaces
{
    public interface IDataModelMapper<TData, TDomain>
    {
        TData MapToModel(TDomain entity);

        TDomain MapToDomain(TData model);
    }
}