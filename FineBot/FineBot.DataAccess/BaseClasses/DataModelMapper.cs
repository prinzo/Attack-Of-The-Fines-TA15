using FineBot.DataAccess.Mappers.Interfaces;

namespace FineBot.DataAccess.BaseClasses
{
    public abstract class DataModelMapper<TData, TDomain> : IDataModelMapper<TData, TDomain>
    {
        public virtual TData MapToModel(TDomain entity)
        {
            throw new System.NotImplementedException();
        }

        public virtual TDomain MapToDomain(TData model)
        {
            throw new System.NotImplementedException();
        }
    }
}