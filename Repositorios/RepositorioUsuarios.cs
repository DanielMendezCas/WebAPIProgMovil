using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPIProgMovil.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private string? connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<IdentityUser?> BuscarUsuarioPorEmail(string normalizedEmail)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                return await conexion.QuerySingleOrDefaultAsync<IdentityUser>
                    ("Usuarios_BuscarPorEmail", new { normalizedEmail },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IdentityUser?> BuscarUsuarioPorUserName(string normalizedUserName)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                return await conexion.QuerySingleOrDefaultAsync<IdentityUser>
                   ("Usuarios_BuscarPorUserName", new { normalizedUserName },
                   commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> Crear(IdentityUser user)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                user.Id = Guid.NewGuid().ToString();
                await conexion.ExecuteAsync
                    ("Usuarios_Crear", new
                    {
                        user.Id,
                        user.Email,
                        user.NormalizedEmail,
                        user.UserName,
                        user.NormalizedUserName,
                        user.PasswordHash
                    },
                    commandType: CommandType.StoredProcedure);
                return user.Id;
            }
        }
    }
}
