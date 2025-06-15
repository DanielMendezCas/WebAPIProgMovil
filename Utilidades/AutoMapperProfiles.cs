using AutoMapper;
using WebAPIProgMovil.DTOs;
using WebAPIProgMovil.Entidades;

namespace WebAPIProgMovil.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearPinDTO, Pin>()
                .ForMember(x => x.Imagen, opciones => opciones.Ignore());
            CreateMap<Pin, PinDTO>();
        }
    }
}
