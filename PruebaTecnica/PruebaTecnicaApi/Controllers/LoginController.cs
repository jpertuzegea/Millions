//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnicaApi.Controllers
{
    [Route("api/Login")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoginController : ControllerBase
    {

        private readonly ILoginServices ILoginServices;

        public LoginController(ILoginServices ILoginervices)
        {
            this.ILoginServices = ILoginervices;
        }


        /// <summary>
        /// Permite iniciar sesion User:jorge Password:123456789
        /// </summary>
        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel<LoginModel>>> LogIn([FromBody] LoginModel LoginModel)
        {
            return await ILoginServices.Login(LoginModel);
        }
         

    }
}
