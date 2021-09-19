using FluentValidation;
using GraphTheory.Entites.HelperEntites;

namespace Validator
{
    public class VertexValidator:AbstractValidator<Vertex>
    {
        public VertexValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}
