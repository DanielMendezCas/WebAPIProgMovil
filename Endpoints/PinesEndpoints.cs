using AutoMapper;
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
            group.MapPost("/", CrearPin).DisableAntiforgery();
            return group;
        }

        static async Task<Created<PinDTO>> CrearPin([FromForm]CrearPinDTO crearPinDTO, 
            IRepositorioPines repositorioPines,
            IMapper mapper, IAlmacenadorArchivos almacenador)
        {
            var pin = mapper.Map<Pin>(crearPinDTO);

            if(crearPinDTO.Imagen is not null)
            {
                var url = await almacenador.Almacenar(contenedor, crearPinDTO.Imagen);
                pin.Imagen = url;
            }


            var id = await repositorioPines.CrearPin(pin);

            var pinDTO = mapper.Map<PinDTO>(pin);

            return TypedResults.Created($"/pines/{id}", pinDTO);
        }
    }
}
