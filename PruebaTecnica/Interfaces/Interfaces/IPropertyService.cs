//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;
using Infraestructure.Entities;

namespace Interfaces.Interfaces
{
    public interface IPropertyService
    {
        /// <summary>
        /// Agrega nuevas propiedades
        /// </summary>
        Task<ResultModel<string>> PropertyAdd(PropertyDto PropertyModel);
         
        
        /// <summary>
        /// Lista todas los propiedades
        /// </summary>
        Task<ResultModel<PropertyDto[]>> SearchPropertys(SearchDto searchDto);

        /// <summary>
        /// Lista todas los propiedades
        /// </summary>
        Task<ResultModel<PropertyDto[]>> PropertyList();

        /// <summary>
        /// Obtiene una propiedad por ID.
        /// </summary>
        Task<ResultModel<PropertyDto>> GetPropertyByPropertyId(int Id);

        /// <summary>
        /// Actualiza una propiedad
        /// </summary>
        Task<ResultModel<string>> PropertyUpdate(PropertyDto PropertyModel);

        /// <summary>
        /// Elimina una propiedad
        /// </summary>
        Task<ResultModel<string>> PropertyDelete(int PropertyId);

    }
}
