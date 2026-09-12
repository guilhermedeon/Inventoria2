using FluentValidation;

namespace Application.Items.ItemVariants.Delete;

public sealed class DeleteItemVariantCommandValidator : AbstractValidator<DeleteItemVariantCommand>
{
    public DeleteItemVariantCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemVariantId).NotEmpty();
    }
}
