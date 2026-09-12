using FluentValidation;

namespace Application.Items.PackDefinitions.Update;

public sealed class UpdatePackDefinitionCommandValidator : AbstractValidator<UpdatePackDefinitionCommand>
{
    public UpdatePackDefinitionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PackDefinitionId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.Sku).MaximumLength(100);
        RuleFor(x => x.Barcode).MaximumLength(100);

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ItemVariantId).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });
    }
}
