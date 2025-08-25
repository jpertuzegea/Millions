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
    [Route("api/Property")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropertyController : ControllerBase
    {

        private readonly IPropertyService IPropertyervices;


        public PropertyController(IPropertyService IPropertyervices)
        {
            this.IPropertyervices = IPropertyervices;
        }


        /// <summary>
        /// Lista Propiedades
        /// </summary>
        [HttpGet("PropertyList")]
        public async Task<ActionResult<ResultModel<PropertyDto[]>>> Property()
        {
            return await IPropertyervices.PropertyList();
        }


        
        /// <summary>
        /// Lista Propiedades
        /// </summary>
        [HttpPost("SearchPropertys")]
        public async Task<ActionResult<ResultModel<PropertyDto[]>>> SearchPropertys([FromBody] SearchDto searchDto)
        {
            return await IPropertyervices.SearchPropertys(searchDto);
        }



        /// <summary>
        /// Agrega nueva Propiedad
        /// </summary>
        [HttpPost("PropertyAdd")]
        public async Task<ActionResult<ResultModel<string>>> PropertyAdd([FromBody] PropertyDto PropertyModel)
        {
            return await IPropertyervices.PropertyAdd(PropertyModel);
        }


        /// <summary>
        /// Obtiene Propiedad por id 
        /// </summary>
        [HttpPost("GetPropertyByPropertyId")]
        public async Task<ActionResult<ResultModel<PropertyDto>>> GetPropertyByPropertyId([FromBody] int PropertyId)
        {
            return await IPropertyervices.GetPropertyByPropertyId(PropertyId);
        }


        /// <summary>
        /// Actualiza Propiedad
        /// </summary>

        [HttpPut("PropertyUpdt")]
        public async Task<ActionResult<ResultModel<string>>> PropertyUpdt([FromBody] PropertyDto PropertyModel)
        {
            return await IPropertyervices.PropertyUpdate(PropertyModel);
        }


        /// <summary>
        /// Elimina Propiedad
        /// </summary>
        [HttpDelete("PropertyDelete/{PropertyId}")]
        public async Task<ActionResult<ResultModel<string>>> PropertyDelete(int PropertyId)
        {
            return await IPropertyervices.PropertyDelete(PropertyId);
        }

    }
}
