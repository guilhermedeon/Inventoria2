using System;
using System.Collections.Generic;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Util;
using Domain.Classifications;
using Domain.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Categories.Create;

public record CategoryCreateCommand
    (
        Guid UserId,
        string Name,
        string Slug,
        Guid? ParentId
    ) : ICommand<Guid>;

public class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

public class CategoryCreateCommandHandler
    (
        IApplicationDbContext context,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext
    )
    : ICommandHandler<CategoryCreateCommand, Guid>
{

    public async Task<Result<Guid>> Handle(CategoryCreateCommand command, CancellationToken cancellationToken)
    {
        Result<User> validateUserResult = await ValidateUser.ValidateAsync(command.UserId, userContext, context, cancellationToken);

        if (validateUserResult.IsFailure)
        {
            return Result.Failure<Guid>(validateUserResult.Error);
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Slug = command.Slug,
            ParentId = command.ParentId,
            CreatedAt = dateTimeProvider.UtcNow
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
