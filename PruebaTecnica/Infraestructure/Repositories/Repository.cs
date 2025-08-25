//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ContextDB ContextDB;


        public Repository(ContextDB _contextdb)
        {
            this.ContextDB = _contextdb;
        }

         

        public void Add(TEntity entity)
        {
            ContextDB.Set<TEntity>().Add(entity);
        }

        public void AddRange(List<TEntity> list)
        {
            ContextDB.Set<TEntity>().AddRange(list);
        }



        public async Task<TEntity> Find(int Id)
        {
            return await ContextDB.Set<TEntity>().FindAsync(Id);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> query)
        {
            return await ContextDB.Set<TEntity>().FirstOrDefaultAsync(query);
        }

      
        public async Task<IEnumerable<TEntity>> Get()
        {
            return await ContextDB.Set<TEntity>().ToListAsync();
        }
         
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query)
        {
            return await ContextDB.Set<TEntity>().Where(query).ToListAsync();
        }
         

        public void Update(TEntity entity)
        {
            ContextDB.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            ContextDB.Entry(entity).State = EntityState.Deleted;
        }

       
        public void RemoveRange(List<TEntity> entity)
        {
            ContextDB.Set<TEntity>().RemoveRange(entity);
        }



    }
}