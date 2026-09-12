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

namespace Application.Items.ItemDefinitions.Update;

internal sealed class UpdateItemDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateItemDefinitionCommand>
{
    public async Task<Result> Handle(UpdateItemDefinitionCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        ItemDefinition? item = await context.ItemDefinitions
            .SingleOrDefaultAsync(i => i.Id == command.ItemDefinitionId && !i.IsDeleted, cancellationToken);

        if (item is null)
        {
            return Result.Failure(ItemDefinitionErrors.NotFound(command.ItemDefinitionId));
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.ItemDefinitions
                .AnyAsync(i => i.Sku == command.Sku && i.Id != command.ItemDefinitionId && !i.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure(ItemDefinitionErrors.SkuNotUnique(command.Sku));
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

        item.Name = command.Name;
        item.Description = command.Description;
        item.Brand = command.Brand;
        item.Manufacturer = command.Manufacturer;
        item.Model = command.Model;
        item.Sku = command.Sku;
        item.CategoryId = command.CategoryId;
        item.UpdatedAt = dateTimeProvider.UtcNow;

        item.Raise(new EntityUpdatedDomainEvent<ItemDefinition>(item.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
