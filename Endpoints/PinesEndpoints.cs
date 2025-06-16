using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPIProgMovil.DTOs;
using WebAPIProgMovil.Entidades;
using WebAPIProgMovil.Repositorios;
using WebAPIProgMovil.Servicios;

namespace WebAPIProgMovil.Endpoints
{
    public static class PinesEndpoints
    {
        private static readonly string contenedor = "pines";

        public static RouteGroupBuilder MapPines(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerPines);
            group.MapGet("/{id:int}", ObtenerPin);
            group.MapPost("/", CrearPin).DisableAntiforgery().RequireAuthorization();
            group.MapPut("/{id:int}", ActualizarPin).DisableAntiforgery();
            group.MapDelete("/{id:int}", EliminarPin);
            return group;
        }

        static async Task<Ok<List<PinDTO>>> ObtenerPines(IRepositorioPines repositorioPines,
            IMapper mapper)
        {
            var pines = await repositorioPines.ObtenerPines();
            var pinesDTO = mapper.Map<List<PinDTO>>(pines);
            return TypedResults.Ok(pinesDTO);
        }

        static async Task<Results<Ok<PinDTO>, NotFound<string>>> ObtenerPin(int id, IRepositorioPines repositorioPines,
            IMapper mapper)
        {
            var pin = await repositorioPines.ObtenerPinPorId(id);

            if (pin is null)
            {
                return TypedResults.NotFound("Pin no encontrado");
            }

            var pinDTO = mapper.Map<PinDTO>(pin);
            return TypedResults.Ok(pinDTO);
        }

        static async Task<Results<Created<PinDTO>, BadRequest<string>>> CrearPin([FromForm] CrearPinDTO crearPinDTO,
            IRepositorioPines repositorioPines,
            IMapper mapper,
            IAlmacenadorArchivos almacenador,
            IServicioUsuarios servicioUsuarios)
        {
            var pin = mapper.Map<Pin>(crearPinDTO);

            if (crearPinDTO.Imagen is not null)
            {
                var url = await almacenador.Almacenar(contenedor, crearPinDTO.Imagen);
                pin.Imagen = url;
            }

            var usuario = await servicioUsuarios.ObtenerUsuario();

            if (usuario is null)
            {
                return TypedResults.BadRequest("");
            }

            pin.UsuarioId = usuario.Id;

            var id = await repositorioPines.CrearPin(pin);

            var pinDTO = mapper.Map<PinDTO>(pin);
            return TypedResults.Created($"/pines/{id}", pinDTO);
        }

        static async Task<Results<NoContent, NotFound>> EliminarPin(int id,
            IRepositorioPines repositorioPines
            )
        {
            var pin = await repositorioPines.ObtenerPinPorId(id);
            
            if(pin is null)
            {
                return TypedResults.NotFound();
            }
            await repositorioPines.EliminarPin(id);
            return TypedResults.NoContent();
        }

        static async Task<Results<Ok<PinDTO>, NotFound<string>, BadRequest<string>>> ActualizarPin(
            int id,
            [FromForm] CrearPinDTO crearPinDTO,
            IRepositorioPines repositorioPines,
            IAlmacenadorArchivos almacenador,
            IMapper mapper,
            IServicioUsuarios servicioUsuarios)
        {
            var pin = await repositorioPines.ObtenerPinPorId(id);
            if (pin is null)
            {
                return TypedResults.NotFound("Pin no encontrado");
            }

            pin.Titulo = crearPinDTO.Titulo;
            pin.Descripcion = crearPinDTO.Descripcion;

            if (crearPinDTO.Imagen is not null)
            {
                var urlImagen = await almacenador.Almacenar(contenedor, crearPinDTO.Imagen);
                pin.Imagen = urlImagen;
            }

            await repositorioPines.ActualizarPin(pin);

            var pinDTO = mapper.Map<PinDTO>(pin);
            return TypedResults.Ok(pinDTO);
        }

    }
}
