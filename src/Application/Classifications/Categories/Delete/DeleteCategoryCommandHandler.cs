using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Categories.Delete;

internal sealed class DeleteCategoryCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure(validateUserResult.Error);
        }

        Category? category = await context.Categories
            .SingleOrDefaultAsync(c => c.Id == command.CategoryId && !c.IsDeleted, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        category.IsDeleted = true;
        category.DeletedAt = dateTimeProvider.UtcNow;

        category.Raise(new EntityDeletedDomainEvent<Category>(category.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
