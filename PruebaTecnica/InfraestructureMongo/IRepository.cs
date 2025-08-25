
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using System.Linq.Expressions;

namespace InfraestructureMongo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query);

        void Add(TEntity entity);
        void AddRange(List<TEntity> list);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entity);

        Task<TEntity> Find(int Id);
        Task<TEntity> Find(Expression<Func<TEntity, bool>> query);


    }
}
