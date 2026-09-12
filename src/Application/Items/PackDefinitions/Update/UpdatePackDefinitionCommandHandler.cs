using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Items;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.PackDefinitions.Update;

internal sealed class UpdatePackDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdatePackDefinitionCommand>
{
    public async Task<Result> Handle(UpdatePackDefinitionCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        PackDefinition? pack = await context.PackDefinitions
            .Include(p => p.Items)
            .SingleOrDefaultAsync(p => p.Id == command.PackDefinitionId && !p.IsDeleted, cancellationToken);

        if (pack is null)
        {
            return Result.Failure(PackDefinitionErrors.NotFound(command.PackDefinitionId));
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.PackDefinitions
                .AnyAsync(p => p.Sku == command.Sku && p.Id != command.PackDefinitionId && !p.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure(PackDefinitionErrors.SkuNotUnique(command.Sku));
            }
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

        if (command.Items is not null)
        {
            foreach (PackItemDto packItem in command.Items)
            {
                bool variantExists = await context.ItemVariants
                    .AnyAsync(v => v.Id == packItem.ItemVariantId && !v.IsDeleted, cancellationToken);

                if (!variantExists)
                {
                    return Result.Failure(PackDefinitionErrors.ItemVariantNotFound(packItem.ItemVariantId));
                }
            }
        }

        pack.Name = command.Name;
        pack.Description = command.Description;
        pack.Sku = command.Sku;
        pack.Barcode = command.Barcode;
        pack.CategoryId = command.CategoryId;
        pack.UpdatedAt = dateTimeProvider.UtcNow;

        if (command.Items is not null)
        {
            pack.Items.Clear();

            foreach (PackItemDto itemDto in command.Items)
            {
                ItemVariant variant = await context.ItemVariants
                    .SingleAsync(v => v.Id == itemDto.ItemVariantId, cancellationToken);

                pack.Items.Add(new PackDefinitionItem
                {
                    PackDefinitionId = pack.Id,
                    PackDefinition = pack,
                    ItemVariantId = itemDto.ItemVariantId,
                    ItemVariant = variant,
                    Quantity = itemDto.Quantity
                });
            }
        }

        pack.Raise(new EntityUpdatedDomainEvent<PackDefinition>(pack.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
