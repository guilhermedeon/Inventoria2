using Application.Abstractions.Authentication;
using Application.Classifications.Categories.Create;
using Application.UnitTests.Abstractions;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Users;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UnitTests.Categories;

public sealed class CreateCategoryCommandHandlerTests : BaseHandlerTest
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static CreateCategoryCommand CreateValidCommand(Guid? parentId = null) =>
        new(UserId, "Electronics", "electronics", parentId);

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenUserIdDoesNotMatchContext()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(Guid.NewGuid());
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreateCategoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.Unauthorized());
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreateCategoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.NotFound(UserId));
    }

    [Fact]
    public async Task Handle_Should_ReturnConflict_WhenSlugAlreadyExists()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        context.Users.Add(new User
        {
            Id = UserId,
            Email = "user@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash"
        });
        context.Categories.Add(new Category
        {
            Id = Guid.NewGuid(),
            Name = "Existing Category",
            Slug = "electronics",
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreateCategoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.SlugNotUnique("electronics"));
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenParentCategoryDoesNotExist()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        context.Users.Add(new User
        {
            Id = UserId,
            Email = "user@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var nonExistentParentId = Guid.NewGuid();
        var handler = new CreateCategoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(parentId: nonExistentParentId), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.ParentNotFound(nonExistentParentId));
    }

    [Fact]
    public async Task Handle_Should_PersistCategoryAndRaiseDomainEvent_WhenValid()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        context.Users.Add(new User
        {
            Id = UserId,
            Email = "user@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);

        var handler = new CreateCategoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        Category category = await context.Categories.SingleAsync(c => c.Id == result.Value);
        category.Name.ShouldBe("Electronics");
        category.Slug.ShouldBe("electronics");
        category.DomainEvents.ShouldContain(e => e is EntityCreatedDomainEvent<Category>);
    }

    [Fact]
    public void Validator_Should_Pass_WhenValid()
    {
        // Arrange
        var validator = new CreateCategoryCommandValidator();
        CreateCategoryCommand command = CreateValidCommand();

        // Act
        TestValidationResult<CreateCategoryCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
