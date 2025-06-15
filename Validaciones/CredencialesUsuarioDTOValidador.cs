using FluentValidation;
using WebAPIProgMovil.DTOs;

namespace WebAPIProgMovil.Validaciones
{
    public class CredencialesUsuarioDTOValidador: AbstractValidator<CredencialesUsuarioDTO>
    {
        public CredencialesUsuarioDTOValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.CampoRequerido).
                                  MaximumLength(256).WithMessage(Utilidades.MaxLengthMensaje).
                                  EmailAddress().WithMessage(Utilidades.EmailMensaje);

            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilidades.CampoRequerido);
        }
    }
}