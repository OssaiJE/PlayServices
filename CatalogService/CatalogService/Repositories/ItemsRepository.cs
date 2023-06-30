using CatalogService.Entities;
using MongoDB.Driver;

namespace CatalogService.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<ItemEntity> dbCollection;
        private readonly FilterDefinitionBuilder<ItemEntity> filterBuilder = Builders<ItemEntity>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://127.0.0.1:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<ItemEntity>(collectionName);
        }

        public async Task<IReadOnlyCollection<ItemEntity>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
    }
}
