//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;

namespace Interfaces.Interfaces
{
    public interface IOwnerService
    {
        /// <summary>
        /// Agrega nuevos propietarios
        /// </summary>
        Task<ResultModel<string>> OwnerAdd(OwnerModel OwnerModel);

        /// <summary>
        /// Lista todos los propietarios
        /// </summary>
        Task<ResultModel<OwnerModel[]>> OwnerList();

        /// <summary>
        /// Obtiene un propietario por ID.
        /// </summary>
        Task<ResultModel<OwnerModel>> GetOwnerByOwnerId(int Id);

        /// <summary>
        /// Actualiza un propietario
        /// </summary>
        Task<ResultModel<string>> OwnerUpdate(OwnerModel OwnerModel);

        /// <summary>
        /// Elimina un propietario
        /// </summary>
        Task<ResultModel<string>> OwnerDelete(int OwnerId);

    }
}
