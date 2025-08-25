
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using MongoDB.Driver;
using System.Linq.Expressions;

namespace InfraestructureMongo
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(MongoContext context, string collectionName)
        {
            _collection = context.GetCollection<TEntity>(collectionName);
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query)
        {
            return await _collection.Find(query).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public void AddRange(List<TEntity> list)
        {
            _collection.InsertMany(list);
        }

        public void Update(TEntity entity)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null) throw new Exception("Entity must have an Id property");

            var id = idProperty.GetValue(entity).ToString();
            _collection.ReplaceOne(Builders<TEntity>.Filter.Eq("Id", id), entity);
        }

        public void Remove(TEntity entity)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null) throw new Exception("Entity must have an Id property");

            var id = idProperty.GetValue(entity).ToString();
            _collection.DeleteOne(Builders<TEntity>.Filter.Eq("Id", id));
        }

        public void RemoveRange(List<TEntity> entities)
        {
            var ids = entities
                .Select(e => typeof(TEntity).GetProperty("Id").GetValue(e).ToString())
                .ToList();

            _collection.DeleteMany(Builders<TEntity>.Filter.In("Id", ids));
        }

        public async Task<TEntity> Find(int Id)
        {
            throw new NotSupportedException("Mongo usa string/ObjectId como clave, no int");
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> query)
        {
            return await _collection.Find(query).FirstOrDefaultAsync();
        }
    }
}  