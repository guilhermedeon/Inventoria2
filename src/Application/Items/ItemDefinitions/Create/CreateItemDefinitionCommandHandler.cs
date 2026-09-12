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

namespace Application.Items.ItemDefinitions.Create;

internal sealed class CreateItemDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreateItemDefinitionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateItemDefinitionCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        if (!string.IsNullOrWhiteSpace(command.Sku))
        {
            bool skuExists = await context.ItemDefinitions
                .AnyAsync(i => i.Sku == command.Sku && !i.IsDeleted, cancellationToken);

            if (skuExists)
            {
                return Result.Failure<Guid>(ItemDefinitionErrors.SkuNotUnique(command.Sku));
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

        ItemDefinition itemDefinition = new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Brand = command.Brand,
            Manufacturer = command.Manufacturer,
            Model = command.Model,
            Sku = command.Sku,
            CategoryId = command.CategoryId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        itemDefinition.Raise(new EntityCreatedDomainEvent<ItemDefinition>(itemDefinition.Id));

        context.ItemDefinitions.Add(itemDefinition);

        await context.SaveChangesAsync(cancellationToken);

        return itemDefinition.Id;
    }
}
