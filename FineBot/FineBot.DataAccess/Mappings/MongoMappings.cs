using FineBot.Entities;
using MongoDB.Bson.Serialization;

namespace FineBot.DataAccess.Mappings
{
    public static class MongoMappings
    {
        public static void SetupMappings()
        {
            BsonClassMap.RegisterClassMap<Fine>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapMember(m => m.Pending);
                    }
                );
        }
    }
}
