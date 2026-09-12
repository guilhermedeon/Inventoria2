using FluentValidation;

namespace Application.Items.ItemDefinitions.Delete;

public sealed class DeleteItemDefinitionCommandValidator : AbstractValidator<DeleteItemDefinitionCommand>
{
    public DeleteItemDefinitionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemDefinitionId).NotEmpty();
    }
}
