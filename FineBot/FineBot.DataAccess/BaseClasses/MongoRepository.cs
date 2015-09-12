using System.Collections.Generic;
using System.Configuration;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FineBot.DataAccess.BaseClasses
{
    public class MongoRepository<TEntity, TData, TIdentifier> : IRepository<TEntity, TData, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {
        private readonly IDataModelMapper<TData, TEntity> dataModelMapper;
        private readonly IMongoDatabase database;

        public MongoRepository(IMongoClient client, IDataModelMapper<TData, TEntity> dataModelMapper)
        {
            this.dataModelMapper = dataModelMapper;
            this.database = client.GetDatabase(ConfigurationManager.AppSettings["MongoDatabase"]);
        }

        public TEntity Get(TIdentifier id)
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).Find(x => x.Id.Equals(id)).FirstOrDefaultAsync().Result;
        }

        public TEntity Find(ISpecification<TEntity> specification)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            return collection.Find(specification.Predicate).FirstOrDefaultAsync().Result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).Find(new BsonDocument()).ToListAsync().Result;
        }

        public IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            var listAsync = collection.Find(specification.Predicate).ToListAsync();

            return listAsync.Result;
        }

        public TEntity Save(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions
                                                                            {
                                                                                IsUpsert = true
                                                                            });

            return entity;
        }

        public void Delete(TIdentifier id)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public void Delete(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }
    }
}