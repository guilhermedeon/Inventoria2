using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Common.DomainEvents;
using Domain.Items;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.PackDefinitions.Delete;

internal sealed class DeletePackDefinitionCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeletePackDefinitionCommand>
{
    public async Task<Result> Handle(DeletePackDefinitionCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        PackDefinition? pack = await context.PackDefinitions
            .SingleOrDefaultAsync(p => p.Id == command.PackDefinitionId && !p.IsDeleted, cancellationToken);

        if (pack is null)
        {
            return Result.Failure(PackDefinitionErrors.NotFound(command.PackDefinitionId));
        }

        pack.IsDeleted = true;
        pack.DeletedAt = dateTimeProvider.UtcNow;

        pack.Raise(new EntityDeletedDomainEvent<PackDefinition>(pack.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
