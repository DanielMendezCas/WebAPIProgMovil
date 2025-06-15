using System.Globalization;

namespace WebAPIProgMovil.DTOs
{
    public class CredencialesUsuarioDTO
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
