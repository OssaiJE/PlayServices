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

        //Get all items respository
        public async Task<IReadOnlyCollection<ItemEntity>> GetAllItems()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        //Get one Item repository
        public async Task<ItemEntity> GetOneItem(Guid id)
        {
            FilterDefinition<ItemEntity> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        //Create Item repository
        public async Task CreateItem(ItemEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        //Update Item repository
        public async Task UpdateItem(ItemEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<ItemEntity> filter = filterBuilder.Eq(existingEntity=> existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        //Delete Item repository
        public async Task RemoveItem(Guid id)
        {
            FilterDefinition<ItemEntity> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
