using FluentValidation;

namespace Application.Items.ItemDefinitions.Create;

public sealed class CreateItemDefinitionCommandValidator : AbstractValidator<CreateItemDefinitionCommand>
{
    public CreateItemDefinitionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.Brand).MaximumLength(100);
        RuleFor(x => x.Manufacturer).MaximumLength(200);
        RuleFor(x => x.Model).MaximumLength(200);
        RuleFor(x => x.Sku).MaximumLength(100);
    }
}
