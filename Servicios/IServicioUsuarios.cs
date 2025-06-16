using Microsoft.AspNetCore.Identity;

namespace WebAPIProgMovil.Servicios
{
    public interface IServicioUsuarios
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}