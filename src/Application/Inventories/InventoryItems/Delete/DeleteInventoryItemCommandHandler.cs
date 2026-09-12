using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.InventoryItems.Delete;

internal sealed class DeleteInventoryItemCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteInventoryItemCommand>
{
    public async Task<Result> Handle(DeleteInventoryItemCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        InventoryItem? item = await context.InventoryItems
            .SingleOrDefaultAsync(i => i.Id == command.InventoryItemId && !i.IsDeleted, cancellationToken);

        if (item is null)
        {
            return Result.Failure(InventoryItemErrors.NotFound(command.InventoryItemId));
        }

        item.IsDeleted = true;
        item.DeletedAt = dateTimeProvider.UtcNow;

        item.Raise(new EntityDeletedDomainEvent<InventoryItem>(item.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
