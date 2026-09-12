using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Tags.Delete;

internal sealed class DeleteTagCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteTagCommand>
{
    public async Task<Result> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        Tag? tag = await context.Tags
            .SingleOrDefaultAsync(t => t.Id == command.TagId && !t.IsDeleted, cancellationToken);

        if (tag is null)
        {
            return Result.Failure(TagErrors.NotFound(command.TagId));
        }

        tag.IsDeleted = true;
        tag.DeletedAt = dateTimeProvider.UtcNow;

        tag.Raise(new EntityDeletedDomainEvent<Tag>(tag.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
