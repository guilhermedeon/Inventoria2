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

namespace Application.Inventories.Inventories.Create;

internal sealed class CreateInventoryCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreateInventoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInventoryCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        if (command.CategoryId.HasValue)
        {
            bool categoryExists = await context.Categories
                .AnyAsync(c => c.Id == command.CategoryId.Value && !c.IsDeleted, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId.Value));
            }
        }

        if (command.ParentInventoryId.HasValue)
        {
            bool parentExists = await context.Inventory
                .AnyAsync(i => i.Id == command.ParentInventoryId.Value && !i.IsDeleted, cancellationToken);

            if (!parentExists)
            {
                return Result.Failure<Guid>(InventoryErrors.ParentNotFound(command.ParentInventoryId.Value));
            }
        }

        Inventory inventory = new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            CategoryId = command.CategoryId,
            ParentInventoryId = command.ParentInventoryId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        inventory.Raise(new EntityCreatedDomainEvent<Inventory>(inventory.Id));

        context.Inventory.Add(inventory);

        await context.SaveChangesAsync(cancellationToken);

        return inventory.Id;
    }
}
