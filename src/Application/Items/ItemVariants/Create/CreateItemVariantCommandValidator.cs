using FluentValidation;

namespace Application.Items.ItemVariants.Create;

public sealed class CreateItemVariantCommandValidator : AbstractValidator<CreateItemVariantCommand>
{
    public CreateItemVariantCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ItemDefinitionId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.Sku).MaximumLength(100);
        RuleFor(x => x.Barcode).MaximumLength(100);
    }
}
