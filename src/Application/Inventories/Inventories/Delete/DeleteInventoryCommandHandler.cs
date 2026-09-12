using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.Inventories.Delete;

internal sealed class DeleteInventoryCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteInventoryCommand>
{
    public async Task<Result> Handle(DeleteInventoryCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        Inventory? inventory = await context.Inventory
            .SingleOrDefaultAsync(i => i.Id == command.InventoryId && !i.IsDeleted, cancellationToken);

        if (inventory is null)
        {
            return Result.Failure(InventoryErrors.NotFound(command.InventoryId));
        }

        inventory.IsDeleted = true;
        inventory.DeletedAt = dateTimeProvider.UtcNow;

        inventory.Raise(new EntityDeletedDomainEvent<Inventory>(inventory.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
