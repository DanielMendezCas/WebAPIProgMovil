using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPIProgMovil.DTOs;
using WebAPIProgMovil.Utilidades;

namespace WebAPIProgMovil.Endpoints
{
    public static class UsuariosEnpoints
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapPost("/registrar", Registrar);
            group.MapPost("/inicioSesion", InicioSesion);
            return group;
        }

        static async Task<Results<Ok<RespuestaAutenticacionDTO>, BadRequest<IEnumerable<IdentityError>>>> 
            Registrar(CredencialesUsuarioDTO credencialesUsuarioDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDTO.UserName,
                Email = credencialesUsuarioDTO.Email,
            };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDTO.Password);

            if (resultado.Succeeded)
            {
                var credencialesRespuesta = ConstruirToken(credencialesUsuarioDTO, configuration);
                return TypedResults.Ok(credencialesRespuesta);
            }
            else
            {
                return TypedResults.BadRequest(resultado.Errors);
            }
        }

        static async Task<Results<Ok<RespuestaAutenticacionDTO>, BadRequest<string>>> InicioSesion
           (
               CredencialesLoginDTO credencialesUsuarioDTO,
               [FromServices] SignInManager<IdentityUser> signInManager,
               [FromServices] UserManager<IdentityUser> userManager,
               IConfiguration configuration
           )
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            if (usuario is null)
            {
                return TypedResults.BadRequest("Credenciales Incorrectas");
            }
            var resultado = await signInManager.CheckPasswordSignInAsync(usuario,
                credencialesUsuarioDTO.Password, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                var tokenDTO = new CredencialesUsuarioDTO
                {
                    Email = usuario.Email!,
                    UserName = usuario.UserName!,
                    Password = "" 
                };
                var respuestaAutenticacion = ConstruirToken(tokenDTO, configuration);
                return TypedResults.Ok(respuestaAutenticacion);
            }
            else
            {
                return TypedResults.BadRequest("Credenciales Incorrectas");
            }
        }

        private static RespuestaAutenticacionDTO ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO,
            IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim("userName", credencialesUsuarioDTO?.UserName),
                new Claim("email", credencialesUsuarioDTO?.Email),
            };

            var llave = Llaves.ObtenerLlave(configuration);
            var creds = new SigningCredentials(llave.First(), SecurityAlgorithms.HmacSha256);

            // Expiracion del token, dependerá del negocio
            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenSeguro = new JwtSecurityToken
                (
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiracion,
                    signingCredentials: creds
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguro);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion,
            };
        }
    }
}