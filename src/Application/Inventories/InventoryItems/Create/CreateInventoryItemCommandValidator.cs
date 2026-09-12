using FluentValidation;

namespace Application.Inventories.InventoryItems.Create;

public sealed class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.InventoryId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(2000);
        RuleFor(x => x.SerialNumber).MaximumLength(200);
        RuleFor(x => x.AssetNumber).MaximumLength(200);

        RuleFor(x => x)
            .Must(x => x.ItemVariantId.HasValue ^ x.PackDefinitionId.HasValue)
            .WithMessage("Either an item variant or a pack definition must be specified, but not both.");
    }
}
