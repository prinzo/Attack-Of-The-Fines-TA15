using FineBot.DataAccess.DataModels;
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
                        cm.SetIgnoreExtraElements(true);
                    }
                );
        }
    }
}
