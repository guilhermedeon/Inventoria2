using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Tags.Create;

internal sealed class CreateTagCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<CreateTagCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        bool slugExists = await context.Tags
            .AnyAsync(t => t.Slug == command.Slug && !t.IsDeleted, cancellationToken);

        if (slugExists)
        {
            return Result.Failure<Guid>(TagErrors.SlugNotUnique(command.Slug));
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Slug = command.Slug,
            CreatedAt = dateTimeProvider.UtcNow
        };

        tag.Raise(new EntityCreatedDomainEvent<Tag>(tag.Id));

        context.Tags.Add(tag);

        await context.SaveChangesAsync(cancellationToken);

        return tag.Id;
    }
}
