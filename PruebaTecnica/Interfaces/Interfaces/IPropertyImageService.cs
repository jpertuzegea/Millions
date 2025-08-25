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
    public interface IPropertyImageService
    {
        /// <summary>
        /// Agrega nuevos propietarios
        /// </summary>
        Task<ResultModel<string>> PropertyImageAdd(PropertyImageDto PropertyImageModel);

        /// <summary>
        /// Lista todos los propietarios
        /// </summary>
        Task<ResultModel<PropertyImageDto[]>> PropertyImageList();
       
        /// <summary>
        /// Lista todos las imagenes de las propiedades por Id 
        /// </summary>
        Task<ResultModel<PropertyImageDto[]>> PropertyImageListByPropertyImageId(int PropertyId);
         

        /// <summary>
        /// Obtiene un propietario por ID.
        /// </summary>
        Task<ResultModel<PropertyImageDto>> GetPropertyImageByPropertyImageId(int Id);

        /// <summary>
        /// Actualiza un propietario
        /// </summary>
        Task<ResultModel<string>> PropertyImageUpdate(PropertyImageDto PropertyImageModel);

        /// <summary>
        /// Elimina un propietario
        /// </summary>
        Task<ResultModel<string>> PropertyImageDelete(int PropertyImageId);

    }
}
