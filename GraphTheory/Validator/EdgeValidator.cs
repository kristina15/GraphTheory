using FluentValidation;
using GraphTheory.Entites.HelperEntites;

namespace Validator
{
    public class EdgeValidator:AbstractValidator<Edge>
    {
        public EdgeValidator()
        {
            RuleFor(e => e).NotNull().WithMessage("Uncorrect edge");
            RuleFor(e => e.From).NotNull().WithMessage("Uncorrect from vertex");
            RuleFor(e => e.To).NotNull().WithMessage("Uncorrect to vertex");
        }
    }
}
