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

namespace Application.Items.PackDefinitions.Create;

internal sealed class CreatePackDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreatePackDefinitionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePackDefinitionCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.PackDefinitions
                .AnyAsync(p => p.Sku == command.Sku && !p.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure<Guid>(PackDefinitionErrors.SkuNotUnique(command.Sku));
            }
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

        if (command.Items is not null)
        {
            foreach (PackItemDto packItem in command.Items)
            {
                bool variantExists = await context.ItemVariants
                    .AnyAsync(v => v.Id == packItem.ItemVariantId && !v.IsDeleted, cancellationToken);

                if (!variantExists)
                {
                    return Result.Failure<Guid>(PackDefinitionErrors.ItemVariantNotFound(packItem.ItemVariantId));
                }
            }
        }

        PackDefinition pack = new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Sku = command.Sku,
            Barcode = command.Barcode,
            CategoryId = command.CategoryId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        if (command.Items is not null)
        {
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

        pack.Raise(new EntityCreatedDomainEvent<PackDefinition>(pack.Id));

        context.PackDefinitions.Add(pack);

        await context.SaveChangesAsync(cancellationToken);

        return pack.Id;
    }
}
