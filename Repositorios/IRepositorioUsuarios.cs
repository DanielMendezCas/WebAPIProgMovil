using Microsoft.AspNetCore.Identity;

namespace WebAPIProgMovil.Repositorios
{
    public interface IRepositorioUsuarios
    {
        Task<IdentityUser?> BuscarUsuarioPorUserName(string userName);
        Task<IdentityUser?> BuscarUsuarioPorEmail(string normalizedEmail);
        Task<string> Crear(IdentityUser user);
    }
}