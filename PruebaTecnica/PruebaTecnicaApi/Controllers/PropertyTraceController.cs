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
    [Route("api/PropertyTrace")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropertyTraceController : ControllerBase
    {

        private readonly IPropertyTraceService IPropertyTraceervices;


        public PropertyTraceController(IPropertyTraceService IPropertyTraceervices)
        {
            this.IPropertyTraceervices = IPropertyTraceervices;
        }


        /// <summary>
        /// Lista Propietarios
        /// </summary>
        [HttpGet("PropertyTraceList")]
        public async Task<ActionResult<ResultModel<PropertyTraceDto[]>>> PropertyTrace()
        {
            return await IPropertyTraceervices.PropertyTraceList();
        }

        [HttpGet("PropertyTraceListByPropertyId/{PropertyId}")]
        public async Task<ActionResult<ResultModel<PropertyTraceDto[]>>> PropertyTraceListByPropertyId(int PropertyId)
        {
            return await IPropertyTraceervices.PropertyTraceListByPropertyId(PropertyId);
        }


        /// <summary>
        /// Agrega Propietarios
        /// </summary>
        [HttpPost("PropertyTraceAdd")]
        public async Task<ActionResult<ResultModel<string>>> PropertyTraceAdd([FromBody] PropertyTraceDto PropertyTraceModel)
        {
            return await IPropertyTraceervices.PropertyTraceAdd(PropertyTraceModel);
        }


        /// <summary>
        /// Obtiene Propietario por id 
        /// </summary>
        [HttpPost("GetPropertyTraceByPropertyTraceId")]
        public async Task<ActionResult<ResultModel<PropertyTraceDto>>> GetPropertyTraceByPropertyTraceId([FromBody] int PropertyTraceId)
        {
            return await IPropertyTraceervices.GetPropertyTraceByPropertyTraceId(PropertyTraceId);
        }


        /// <summary>
        /// Actualiza Propietarios
        /// </summary>

        [HttpPut("PropertyTraceUpdt")]
        public async Task<ActionResult<ResultModel<string>>> PropertyTraceUpdt([FromBody] PropertyTraceDto PropertyTraceModel)
        {
            return await IPropertyTraceervices.PropertyTraceUpdate(PropertyTraceModel);
        }


        /// <summary>
        /// Elimina Propietarios
        /// </summary>
        [HttpDelete("PropertyTraceDelete/{PropertyTraceId}")]
        public async Task<ActionResult<ResultModel<string>>> PropertyTraceDelete(int PropertyTraceId)
        {
            return await IPropertyTraceervices.PropertyTraceDelete(PropertyTraceId);
        }

    }
}
