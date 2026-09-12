using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Tags.Update;

internal sealed class UpdateTagCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateTagCommand>
{
    public async Task<Result> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
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

        bool slugExists = await context.Tags
            .AnyAsync(t => t.Slug == command.Slug && t.Id != command.TagId && !t.IsDeleted, cancellationToken);

        if (slugExists)
        {
            return Result.Failure(TagErrors.SlugNotUnique(command.Slug));
        }

        tag.Name = command.Name;
        tag.Slug = command.Slug;
        tag.UpdatedAt = dateTimeProvider.UtcNow;

        tag.Raise(new EntityUpdatedDomainEvent<Tag>(tag.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
