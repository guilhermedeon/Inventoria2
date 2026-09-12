using FluentValidation;

namespace Application.Inventories.Inventories.Delete;

public sealed class DeleteInventoryCommandValidator : AbstractValidator<DeleteInventoryCommand>
{
    public DeleteInventoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.InventoryId).NotEmpty();
    }
}
