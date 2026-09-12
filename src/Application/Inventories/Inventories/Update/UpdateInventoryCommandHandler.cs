using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.Inventories.Update;

internal sealed class UpdateInventoryCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken)
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

        if (command.ParentInventoryId == command.InventoryId)
        {
            return Result.Failure(InventoryErrors.CannotBeParentOfItself());
        }

        if (command.CategoryId.HasValue)
        {
            bool categoryExists = await context.Categories
                .AnyAsync(c => c.Id == command.CategoryId.Value && !c.IsDeleted, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure(CategoryErrors.NotFound(command.CategoryId.Value));
            }
        }

        if (command.ParentInventoryId.HasValue)
        {
            bool parentExists = await context.Inventory
                .AnyAsync(i => i.Id == command.ParentInventoryId.Value && !i.IsDeleted, cancellationToken);

            if (!parentExists)
            {
                return Result.Failure(InventoryErrors.ParentNotFound(command.ParentInventoryId.Value));
            }
        }

        inventory.Name = command.Name;
        inventory.Description = command.Description;
        inventory.CategoryId = command.CategoryId;
        inventory.ParentInventoryId = command.ParentInventoryId;
        inventory.UpdatedAt = dateTimeProvider.UtcNow;

        inventory.Raise(new EntityUpdatedDomainEvent<Inventory>(inventory.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
