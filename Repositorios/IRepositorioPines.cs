using WebAPIProgMovil.Entidades;

namespace WebAPIProgMovil.Repositorios
{
    public interface IRepositorioPines
    {
        Task ActualizarPin(Pin pin);
        Task<int> CrearPin(Pin pin);
        Task EliminarPin(int id);
        Task<List<Pin>> ObtenerPines();
        Task<Pin?> ObtenerPinPorId(int id);
    }
}