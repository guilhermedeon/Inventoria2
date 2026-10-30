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

        Dictionary<Guid, ItemVariant> variantsById = [];
        if (command.Items is not null && command.Items.Count > 0)
        {
            var variantIds = command.Items.Select(i => i.ItemVariantId).Distinct().ToList();

            List<ItemVariant> variants = await context.ItemVariants
                .Where(v => variantIds.Contains(v.Id) && !v.IsDeleted)
                .ToListAsync(cancellationToken);

            if (variants.Count != variantIds.Count)
            {
                var foundIds = variants.Select(v => v.Id).ToHashSet();
                Guid missingVariantId = variantIds.First(id => !foundIds.Contains(id));
                return Result.Failure<Guid>(PackDefinitionErrors.ItemVariantNotFound(missingVariantId));
            }

            variantsById = variants.ToDictionary(v => v.Id);
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
                pack.Items.Add(new PackDefinitionItem
                {
                    PackDefinitionId = pack.Id,
                    PackDefinition = pack,
                    ItemVariantId = itemDto.ItemVariantId,
                    ItemVariant = variantsById[itemDto.ItemVariantId],
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
