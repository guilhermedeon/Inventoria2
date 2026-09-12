using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Common.DomainEvents;
using Domain.Items;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemDefinitions.Delete;

internal sealed class DeleteItemDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteItemDefinitionCommand>
{
    public async Task<Result> Handle(DeleteItemDefinitionCommand command, CancellationToken cancellationToken)
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

        item.IsDeleted = true;
        item.DeletedAt = dateTimeProvider.UtcNow;

        item.Raise(new EntityDeletedDomainEvent<ItemDefinition>(item.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
