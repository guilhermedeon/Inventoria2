using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Categories.Update;

internal sealed class UpdateCategoryCommandHandler
(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
)
: ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
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

        if (command.ParentId == command.CategoryId)
        {
            return Result.Failure(CategoryErrors.CannotBeParentOfItself());
        }

        if (command.ParentId.HasValue)
        {
            bool parentExists = await context.Categories
                .AnyAsync(c => c.Id == command.ParentId.Value && !c.IsDeleted, cancellationToken);

            if (!parentExists)
            {
                return Result.Failure(CategoryErrors.ParentNotFound(command.ParentId.Value));
            }
        }

        bool slugExists = await context.Categories
            .AnyAsync(c => c.Slug == command.Slug && c.Id != command.CategoryId && !c.IsDeleted, cancellationToken);

        if (slugExists)
        {
            return Result.Failure(CategoryErrors.SlugNotUnique(command.Slug));
        }

        category.Name = command.Name;
        category.Slug = command.Slug;
        category.ParentId = command.ParentId;
        category.UpdatedAt = dateTimeProvider.UtcNow;

        category.Raise(new EntityUpdatedDomainEvent<Category>(category.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
