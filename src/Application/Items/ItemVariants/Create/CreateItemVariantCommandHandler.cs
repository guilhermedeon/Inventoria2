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

namespace Application.Items.ItemVariants.Create;

internal sealed class CreateItemVariantCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreateItemVariantCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateItemVariantCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        bool itemDefinitionExists = await context.ItemDefinitions
            .AnyAsync(i => i.Id == command.ItemDefinitionId && !i.IsDeleted, cancellationToken);

        if (!itemDefinitionExists)
        {
            return Result.Failure<Guid>(ItemVariantErrors.ItemDefinitionNotFound(command.ItemDefinitionId));
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.ItemVariants
                .AnyAsync(v => v.Sku == command.Sku && !v.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure<Guid>(ItemVariantErrors.SkuNotUnique(command.Sku));
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

        ItemDefinition itemDefinition = await context.ItemDefinitions
            .SingleAsync(i => i.Id == command.ItemDefinitionId, cancellationToken);

        ItemVariant variant = new()
        {
            Id = Guid.NewGuid(),
            ItemDefinitionId = command.ItemDefinitionId,
            ItemDefinition = itemDefinition,
            Name = command.Name,
            Description = command.Description,
            Sku = command.Sku,
            Barcode = command.Barcode,
            CategoryId = command.CategoryId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        variant.Raise(new EntityCreatedDomainEvent<ItemVariant>(variant.Id));

        context.ItemVariants.Add(variant);

        await context.SaveChangesAsync(cancellationToken);

        return variant.Id;
    }
}
