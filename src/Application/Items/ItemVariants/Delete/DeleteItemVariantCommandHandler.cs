using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Common.DomainEvents;
using Domain.Items;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemVariants.Delete;

internal sealed class DeleteItemVariantCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteItemVariantCommand>
{
    public async Task<Result> Handle(DeleteItemVariantCommand command, CancellationToken cancellationToken)
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

        variant.IsDeleted = true;
        variant.DeletedAt = dateTimeProvider.UtcNow;

        variant.Raise(new EntityDeletedDomainEvent<ItemVariant>(variant.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
