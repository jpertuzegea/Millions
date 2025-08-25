//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace InfraestructureMongo
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoContext _context;

        public MongoUnitOfWork(MongoContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var collectionName = typeof(TEntity).Name;
            return new MongoRepository<TEntity>(_context, collectionName);
        }

        public bool SaveChanges()
        {
            return true;
        }


    }
}
