using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jugueteria.Models.Validations
{
    public class ToysValidator : AbstractValidator<Toys>
    {
        public ToysValidator()
        {
            RuleFor(x => x.Nombre)
              .MaximumLength(50).WithMessage("Maximo 50 caracteters")
              .NotEmpty().WithMessage("El nombre es requerido");

            RuleFor(x => x.Descripcion)
               .MaximumLength(100).WithMessage("Maximo 100 caracteres")
               .NotEmpty().WithMessage("La descripción es requerida");

            RuleFor(x => x.Compañía)
                .MaximumLength(50).WithMessage("Maximo 50 caracteres")
                .NotEmpty().WithMessage("La compañia es requerida");

            RuleFor(x => x.RestriccionEdad)
                .LessThanOrEqualTo(100).WithMessage("Maximo 100")
                .NotEmpty().WithMessage("La restricción es requerida");

            RuleFor(x => x.Precio)
                .LessThanOrEqualTo(1000).WithMessage("Maximo 1000")
                .GreaterThanOrEqualTo(1).WithMessage("Debe ser mayor o igual a 1")
                .NotEmpty().WithMessage("El precio es requerido");
                





        }



    }
}
