using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Pcf.Administration.DataAccess
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public IMongoDatabase Database => _database;

        public MongoDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.Connection);
            _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }
    }
}