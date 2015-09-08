using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using MongoDB.Bson.Serialization;

namespace FineBot.DataAccess.Mappings
{
    public static class MongoMappings
    {
        public static void SetupMappings()
        {
            BsonClassMap.RegisterClassMap<FineDataModel>(
                    cm =>
                    {
                        cm.AutoMap();
                    }
                );
        }
    }
}
