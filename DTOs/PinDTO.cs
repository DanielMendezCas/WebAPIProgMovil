namespace WebAPIProgMovil.DTOs
{
    public class PinDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string Categoria { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? Imagen { get; set; }
        public bool EsPublico { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
    }
}
