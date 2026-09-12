using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Items;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.InventoryItems.Create;

internal sealed class CreateInventoryItemCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreateInventoryItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInventoryItemCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        bool inventoryExists = await context.Inventory
            .AnyAsync(i => i.Id == command.InventoryId && !i.IsDeleted, cancellationToken);

        if (!inventoryExists)
        {
            return Result.Failure<Guid>(InventoryItemErrors.InventoryNotFound(command.InventoryId));
        }

        if (command.ItemVariantId.HasValue)
        {
            bool variantExists = await context.ItemVariants
                .AnyAsync(v => v.Id == command.ItemVariantId.Value && !v.IsDeleted, cancellationToken);

            if (!variantExists)
            {
                return Result.Failure<Guid>(InventoryItemErrors.ItemVariantNotFound(command.ItemVariantId.Value));
            }
        }
        else if (command.PackDefinitionId.HasValue)
        {
            bool packExists = await context.PackDefinitions
                .AnyAsync(p => p.Id == command.PackDefinitionId.Value && !p.IsDeleted, cancellationToken);

            if (!packExists)
            {
                return Result.Failure<Guid>(InventoryItemErrors.PackDefinitionNotFound(command.PackDefinitionId.Value));
            }
        }
        else
        {
            return Result.Failure<Guid>(InventoryItemErrors.VariantOrPackRequired());
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

        Inventory inventory = await context.Inventory
            .SingleAsync(i => i.Id == command.InventoryId, cancellationToken);

        InventoryItem item = new()
        {
            Id = Guid.NewGuid(),
            InventoryId = command.InventoryId,
            Inventory = inventory,
            ItemVariantId = command.ItemVariantId,
            PackDefinitionId = command.PackDefinitionId,
            Quantity = command.Quantity,
            Notes = command.Notes,
            SerialNumber = command.SerialNumber,
            AssetNumber = command.AssetNumber,
            CategoryId = command.CategoryId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        item.Raise(new EntityCreatedDomainEvent<InventoryItem>(item.Id));

        context.InventoryItems.Add(item);

        await context.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
