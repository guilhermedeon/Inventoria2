using FluentValidation;

namespace Application.Inventories.InventoryItems.Delete;

public sealed class DeleteInventoryItemCommandValidator : AbstractValidator<DeleteInventoryItemCommand>
{
    public DeleteInventoryItemCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.InventoryItemId).NotEmpty();
    }
}
