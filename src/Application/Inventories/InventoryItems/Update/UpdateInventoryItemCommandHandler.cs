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

namespace Application.Inventories.InventoryItems.Update;

internal sealed class UpdateInventoryItemCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateInventoryItemCommand>
{
    public async Task<Result> Handle(UpdateInventoryItemCommand command, CancellationToken cancellationToken)
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

        bool inventoryExists = await context.Inventory
            .AnyAsync(i => i.Id == command.InventoryId && !i.IsDeleted, cancellationToken);

        if (!inventoryExists)
        {
            return Result.Failure(InventoryItemErrors.InventoryNotFound(command.InventoryId));
        }

        if (command.ItemVariantId.HasValue)
        {
            bool variantExists = await context.ItemVariants
                .AnyAsync(v => v.Id == command.ItemVariantId.Value && !v.IsDeleted, cancellationToken);

            if (!variantExists)
            {
                return Result.Failure(InventoryItemErrors.ItemVariantNotFound(command.ItemVariantId.Value));
            }
        }
        else if (command.PackDefinitionId.HasValue)
        {
            bool packExists = await context.PackDefinitions
                .AnyAsync(p => p.Id == command.PackDefinitionId.Value && !p.IsDeleted, cancellationToken);

            if (!packExists)
            {
                return Result.Failure(InventoryItemErrors.PackDefinitionNotFound(command.PackDefinitionId.Value));
            }
        }
        else
        {
            return Result.Failure(InventoryItemErrors.VariantOrPackRequired());
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

        item.InventoryId = command.InventoryId;
        item.ItemVariantId = command.ItemVariantId;
        item.PackDefinitionId = command.PackDefinitionId;
        item.Quantity = command.Quantity;
        item.Notes = command.Notes;
        item.SerialNumber = command.SerialNumber;
        item.AssetNumber = command.AssetNumber;
        item.CategoryId = command.CategoryId;
        item.UpdatedAt = dateTimeProvider.UtcNow;

        item.Raise(new EntityUpdatedDomainEvent<InventoryItem>(item.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
