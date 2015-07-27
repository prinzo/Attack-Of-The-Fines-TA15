using System.Collections.Generic;
using System.Threading.Tasks;
using FineBot.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FineBot.DataAccess.BaseClasses
{
    public class MongoRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {
        private IMongoDatabase database;

        public MongoRepository(IMongoClient client)
        {
            //TODO: put this in config
            this.database = client.GetDatabase("fines");
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