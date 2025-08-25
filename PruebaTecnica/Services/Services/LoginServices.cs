//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Interfaces.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly IConfiguration configuration;

        public LoginServices(IConfiguration _configuration)
        {
            configuration = _configuration;
        }



        /// <inheritdoc />
        public async Task<ResultModel<LoginModel>> Login(LoginModel LoginModel)
        {
            ResultModel<LoginModel> ResultModel = new ResultModel<LoginModel>();

            try
            {
                if (LoginModel.UserName == null || LoginModel.Password == null)
                {
                    ResultModel.HasError = false;
                    ResultModel.Data = null;
                    ResultModel.Messages = "Usuario y Clave son requeridos";
                    return ResultModel;
                }

                if (LoginModel.UserName.ToLower() == "Jorge".ToLower() || LoginModel.Password == "123456789")
                {
                    LoginModel.IsLogued = true;
                    LoginModel.Token = BuildToken();
                    LoginModel.Password = "";

                    ResultModel.HasError = false;
                    ResultModel.Data = LoginModel;
                    ResultModel.Messages = "Usuario Logueado Con Exito";
                }
                else
                {
                    LoginModel.IsLogued = false;
                    LoginModel.Token = "";
                    LoginModel.Password = "";

                    ResultModel.HasError = true;
                    ResultModel.Data = LoginModel;
                    ResultModel.Messages = "Usuario NO Logueado";

                }

                return ResultModel;
            }
            catch (Exception Error)
            {
                ResultModel.HasError = true;
                ResultModel.Messages = $"Error Técnico Al Iniciar Sesion: {Error.Message}";
                ResultModel.Data = null;
                ResultModel.ExceptionMessage = Error.ToString();

                return ResultModel;
            }
        }

       
        private string BuildToken()
        {
            int user = 1;

            var Claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, "jpertuzegea@hotmail.com"),

                new Claim("UserId", user.ToString()),
                new Claim("UserEmail", "jpertuzegea@hotmail.com"),
                new Claim("UserFullName", "Jorge David Pertuz Egea"),
                new Claim("UserNetwork", "JpertuzEgea"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JWTAuthentication JWTAuthenticationSection = configuration.GetSection("JWTAuthentication").Get<JWTAuthentication>();

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTAuthenticationSection.Secret));
            var Credenciales = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            DateTime Expiration = DateTime.Now.AddMinutes(JWTAuthenticationSection.ExpirationInMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "",
               audience: "",
               claims: Claims,
               expires: Expiration,
               signingCredentials: Credenciales,
               notBefore: DateTime.Now.AddMilliseconds(2)
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
         
    }
}
