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

namespace Application.Items.ItemVariants.Update;

internal sealed class UpdateItemVariantCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateItemVariantCommand>
{
    public async Task<Result> Handle(UpdateItemVariantCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        ItemVariant? variant = await context.ItemVariants
            .SingleOrDefaultAsync(v => v.Id == command.ItemVariantId && !v.IsDeleted, cancellationToken);

        if (variant is null)
        {
            return Result.Failure(ItemVariantErrors.NotFound(command.ItemVariantId));
        }

        bool itemDefinitionExists = await context.ItemDefinitions
            .AnyAsync(i => i.Id == command.ItemDefinitionId && !i.IsDeleted, cancellationToken);

        if (!itemDefinitionExists)
        {
            return Result.Failure(ItemVariantErrors.ItemDefinitionNotFound(command.ItemDefinitionId));
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.ItemVariants
                .AnyAsync(v => v.Sku == command.Sku && v.Id != command.ItemVariantId && !v.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure(ItemVariantErrors.SkuNotUnique(command.Sku));
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

        variant.ItemDefinitionId = command.ItemDefinitionId;
        variant.Name = command.Name;
        variant.Description = command.Description;
        variant.Sku = command.Sku;
        variant.Barcode = command.Barcode;
        variant.CategoryId = command.CategoryId;
        variant.UpdatedAt = dateTimeProvider.UtcNow;

        variant.Raise(new EntityUpdatedDomainEvent<ItemVariant>(variant.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
