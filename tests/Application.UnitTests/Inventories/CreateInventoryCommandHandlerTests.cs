using Application.Abstractions.Authentication;
using Application.Inventories.Inventories.Create;
using Application.UnitTests.Abstractions;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UnitTests.Inventories;

public sealed class CreateInventoryCommandHandlerTests : BaseHandlerTest
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static CreateInventoryCommand CreateValidCommand(Guid? categoryId = null, Guid? parentInventoryId = null) =>
        new(UserId, "Main Warehouse", "Primary warehouse storage", categoryId, parentInventoryId);

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenUserIdDoesNotMatchContext()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(Guid.NewGuid());
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreateInventoryCommandHandler(context, dateTimeProvider, userContext);

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

        var handler = new CreateInventoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.NotFound(UserId));
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenCategoryDoesNotExist()
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

        var missingCategoryId = Guid.NewGuid();
        var handler = new CreateInventoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(categoryId: missingCategoryId), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(missingCategoryId));
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenParentInventoryDoesNotExist()
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

        var missingParentInventoryId = Guid.NewGuid();
        var handler = new CreateInventoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(parentInventoryId: missingParentInventoryId), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(InventoryErrors.ParentNotFound(missingParentInventoryId));
    }

    [Fact]
    public async Task Handle_Should_PersistInventoryAndRaiseDomainEvent_WhenValid()
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

        var handler = new CreateInventoryCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        Inventory inventory = await context.Inventory.SingleAsync(i => i.Id == result.Value);
        inventory.Name.ShouldBe("Main Warehouse");
        inventory.Description.ShouldBe("Primary warehouse storage");
        inventory.DomainEvents.ShouldContain(e => e is EntityCreatedDomainEvent<Inventory>);
    }
}
