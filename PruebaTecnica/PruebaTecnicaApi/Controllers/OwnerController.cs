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
    [Route("api/Owner")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OwnerController : ControllerBase
    {

        private readonly IOwnerService IOwnerervices;


        public OwnerController(IOwnerService IOwnerervices)
        {
            this.IOwnerervices = IOwnerervices;
        }


        /// <summary>
        /// Lista Propietarios
        /// </summary>
        [HttpGet("OwnerList")]
        public async Task<ActionResult<ResultModel<OwnerModel[]>>> Owner()
        {
            return await IOwnerervices.OwnerList();
        }


        /// <summary>
        /// Agrega Propietarios
        /// </summary>
        [HttpPost("OwnerAdd")]
        public async Task<ActionResult<ResultModel<string>>> OwnerAdd([FromForm()] OwnerModel OwnerModel)
        {
            return await IOwnerervices.OwnerAdd(OwnerModel);
        }


        /// <summary>
        /// Obtiene Propietario por id 
        /// </summary>
        [HttpPost("GetOwnerByOwnerId")]
        public async Task<ActionResult<ResultModel<OwnerModel>>> GetOwnerByOwnerId([FromBody] int OwnerId)
        {
            return await IOwnerervices.GetOwnerByOwnerId(OwnerId);
        }


        /// <summary>
        /// Actualiza Propietarios
        /// </summary>

        [HttpPut("OwnerUpdt")]
        public async Task<ActionResult<ResultModel<string>>> OwnerUpdt([FromBody] OwnerModel OwnerModel)
        {
            return await IOwnerervices.OwnerUpdate(OwnerModel);
        }


        /// <summary>
        /// Elimina Propietarios
        /// </summary>
        [HttpDelete("OwnerDelete/{OwnerId}")]
        public async Task<ActionResult<ResultModel<string>>> OwnerDelete(int OwnerId)
        {
            return await IOwnerervices.OwnerDelete(OwnerId);
        }

    }
}
