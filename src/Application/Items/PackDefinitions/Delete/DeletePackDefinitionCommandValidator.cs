using FluentValidation;

namespace Application.Items.PackDefinitions.Delete;

public sealed class DeletePackDefinitionCommandValidator : AbstractValidator<DeletePackDefinitionCommand>
{
    public DeletePackDefinitionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PackDefinitionId).NotEmpty();
    }
}
