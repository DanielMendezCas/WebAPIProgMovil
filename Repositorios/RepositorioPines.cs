using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;
using WebAPIProgMovil.DTOs;
using WebAPIProgMovil.Entidades;

namespace WebAPIProgMovil.Repositorios
{
    public class RepositorioPines : IRepositorioPines
    {
        private string? connectionString;
        public RepositorioPines(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }
        public async Task<List<Pin>> ObtenerPines()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var pines = await conexion.QueryAsync<Pin>("Pin_ObtenerPines",
                    commandType: CommandType.StoredProcedure);
                return pines.ToList();
            }
        }
        public async Task<Pin?> ObtenerPinPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var pin = await conexion.QueryFirstOrDefaultAsync<Pin>("Pin_ObtenerPinId", new { id },
                    commandType: CommandType.StoredProcedure);
                return pin;
            }
        }

        public async Task<int> CrearPin(Pin pin)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>("Pin_Crear",
                    new
                    {
                        pin.Titulo,
                        pin.Descripcion,
                        pin.Categoria,
                        pin.Latitud,
                        pin.Longitud,
                        pin.Imagen,
                        pin.EsPublico,
                        pin.UsuarioId
                    },
                    commandType: CommandType.StoredProcedure);
                pin.Id = id;
                return id;
            }
        }

        public async Task ActializarPin(Pin pin)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync
                    (
                        "Pin_Actualizar", pin, commandType: CommandType.StoredProcedure
                    );
            }
        }

        public async Task EliminarPin(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync
                (
                    "Pin_Eliminar", new { id },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}

