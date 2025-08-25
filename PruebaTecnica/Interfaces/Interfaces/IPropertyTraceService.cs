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
    public interface IPropertyTraceService
    {
        /// <summary>
        /// Agrega nuevas Trazas
        /// </summary>
        Task<ResultModel<string>> PropertyTraceAdd(PropertyTraceDto PropertyTraceModel);

        /// <summary>
        /// Lista todos los Trazas
        /// </summary>
        Task<ResultModel<PropertyTraceDto[]>> PropertyTraceList();

        /// <summary>
        /// Lista todos los Trazas por PropertyId
        /// </summary>
        Task<ResultModel<PropertyTraceDto[]>> PropertyTraceListByPropertyId(int PropertyId);

        /// <summary>
        /// Obtiene una Trazas por ID.
        /// </summary>
        Task<ResultModel<PropertyTraceDto>> GetPropertyTraceByPropertyTraceId(int Id);

        /// <summary>
        /// Actualiza un Trazas
        /// </summary>
        Task<ResultModel<string>> PropertyTraceUpdate(PropertyTraceDto PropertyTraceModel);

        /// <summary>
        /// Elimina un Trazas
        /// </summary>
        Task<ResultModel<string>> PropertyTraceDelete(int PropertyTraceId);

    }
}
