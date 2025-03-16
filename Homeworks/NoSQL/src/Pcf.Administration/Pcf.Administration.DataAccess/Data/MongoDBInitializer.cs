using System.Collections.Generic;
using MongoDB.Driver;
using Pcf.Administration.Core.Domain;

namespace Pcf.Administration.DataAccess.Data
{
    public class MongoDBInitializer : IDbInitializer
    {
        private readonly MongoDBContext _dbContext;

        public MongoDBInitializer(MongoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeDb()
        {
            ClearDb();

            InitializeDbCollection(MongoDBSettings.EMPLOYEES_COLLECTION_NAME, FakeDataFactory.Employees);
            InitializeDbCollection(MongoDBSettings.ROLES_COLLECTION_NAME, FakeDataFactory.Roles);
        }

        private void InitializeDbCollection<T>(string collectionName, List<T> values) where T : BaseEntity
        {
            _dbContext.Database.CreateCollection(collectionName);

            var collection = _dbContext.Database.GetCollection<T>(collectionName);
            collection.InsertMany(values);
        }

        private void ClearDb()
        {
            var dbCollections = _dbContext.Database.ListCollectionNames().ToList();
            foreach (var collection in dbCollections)
            {
                _dbContext.Database.DropCollection(collection);
            }
        }
    }
}