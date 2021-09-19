using FluentValidation;
using FluentValidation.Results;
using GraphTheory.Entites.HelperEntites;

namespace GraphTheory.Validators
{
    public class EdgeValidator:AbstractValidator<Edge>
    {
        public EdgeValidator()
        {
            RuleFor(e => e.From).NotNull().WithMessage("Uncorrect 'from' vertex");
            RuleFor(e => e.To).NotNull().WithMessage("Uncorrect 'to' vertex");
        }

        public override ValidationResult Validate(ValidationContext<Edge> context)
        {
            return base.Validate(context);
        }
    }
}
