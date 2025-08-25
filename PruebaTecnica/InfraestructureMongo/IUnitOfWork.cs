
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace InfraestructureMongo
{
    public interface IUnitOfWork  
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    
    }
}
