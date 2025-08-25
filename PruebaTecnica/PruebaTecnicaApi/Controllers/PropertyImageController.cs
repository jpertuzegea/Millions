//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnicaApi.Controllers
{
    [Route("api/PropertyImage")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropertyImageController : ControllerBase
    {

        private readonly IPropertyImageService IPropertyImageervices;


        public PropertyImageController(IPropertyImageService IPropertyImageervices)
        {
            this.IPropertyImageervices = IPropertyImageervices;
        }


        /// <summary>
        /// Lista Propietarios
        /// </summary>
        [HttpGet("PropertyImageList")]
        public async Task<ActionResult<ResultModel<PropertyImageDto[]>>> PropertyImage()
        {
            return await IPropertyImageervices.PropertyImageList();
        }


        [HttpGet("GetAllPropertyImagesByIdProperty/{PropertyId}")]
        public async Task<ActionResult<ResultModel<PropertyImageDto[]>>> PropertyImageListByPropertyImageId(int PropertyId)
        {
            return await IPropertyImageervices.PropertyImageListByPropertyImageId(PropertyId);
        }


        /// <summary>
        /// Agrega Propietarios
        /// </summary>
        [HttpPost("PropertyImageAdd")]
        public async Task<ActionResult<ResultModel<string>>> PropertyImageAdd([FromForm()] PropertyImageDto PropertyImageModel)
        {
            return await IPropertyImageervices.PropertyImageAdd(PropertyImageModel);
        }


        /// <summary>
        /// Obtiene Propietario por id 
        /// </summary>
        [HttpPost("GetPropertyImageByPropertyImageId")]
        public async Task<ActionResult<ResultModel<PropertyImageDto>>> GetPropertyImageByPropertyImageId([FromBody] int PropertyImageId)
        {
            return await IPropertyImageervices.GetPropertyImageByPropertyImageId(PropertyImageId);
        }


        /// <summary>
        /// Actualiza Propietarios
        /// </summary>

        [HttpPut("PropertyImageUpdt")]
        public async Task<ActionResult<ResultModel<string>>> PropertyImageUpdt([FromForm] PropertyImageDto PropertyImageModel)
        {
            return await IPropertyImageervices.PropertyImageUpdate(PropertyImageModel);
        }


        /// <summary>
        /// Elimina Propietarios
        /// </summary>
        [HttpDelete("PropertyImageDelete/{PropertyImageId}")]
        public async Task<ActionResult<ResultModel<string>>> PropertyImageDelete(int PropertyImageId)
        {
            return await IPropertyImageervices.PropertyImageDelete(PropertyImageId);
        }

    }
}
