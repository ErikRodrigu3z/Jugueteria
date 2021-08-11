using FluentValidation;
using Jugueteria.Models.Segurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jugueteria.Models.Validations
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El Email es requerido")
                .EmailAddress().WithMessage("Debe ser un Email valido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El Password es requerido");

        }
    }
}
