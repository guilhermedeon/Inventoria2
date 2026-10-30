using Application.Abstractions.Authentication;
using Application.Items.PackDefinitions;
using Application.Items.PackDefinitions.Create;
using Application.UnitTests.Abstractions;
using Domain.Classifications;
using Domain.Common.DomainEvents;
using Domain.Items;
using Domain.Users;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UnitTests.Packs;

public sealed class CreatePackDefinitionCommandHandlerTests : BaseHandlerTest
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static CreatePackDefinitionCommand CreateValidCommand(Guid? categoryId = null, List<PackItemDto>? items = null) =>
        new(
            UserId,
            "Starter Pack",
            "A test pack definition",
            "SKU-PACK-01",
            "BAR-PACK-01",
            categoryId,
            items
        );

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenUserIdDoesNotMatchContext()
    {
        // Arrange
        await using TestDbContext context = CreateDbContext();
        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(Guid.NewGuid());
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

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

        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(UserErrors.NotFound(UserId));
    }

    [Fact]
    public async Task Handle_Should_ReturnConflict_WhenSkuAlreadyExists()
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
        context.PackDefinitions.Add(new PackDefinition
        {
            Id = Guid.NewGuid(),
            Name = "Existing Pack",
            Sku = "SKU-PACK-01",
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(PackDefinitionErrors.SkuNotUnique("SKU-PACK-01"));
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

        var nonExistentCategoryId = Guid.NewGuid();
        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(categoryId: nonExistentCategoryId), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(nonExistentCategoryId));
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenVariantDoesNotExist()
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

        var missingVariantId = Guid.NewGuid();
        var items = new List<PackItemDto> { new(missingVariantId, 2) };
        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(items: items), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(PackDefinitionErrors.ItemVariantNotFound(missingVariantId));
    }

    [Fact]
    public async Task Handle_Should_PersistPackDefinitionAndItems_WhenValid()
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

        var itemDef = new ItemDefinition
        {
            Id = Guid.NewGuid(),
            Name = "Water Bottle",
            CreatedAt = DateTime.UtcNow
        };
        context.ItemDefinitions.Add(itemDef);

        var variant = new ItemVariant
        {
            Id = Guid.NewGuid(),
            ItemDefinitionId = itemDef.Id,
            ItemDefinition = itemDef,
            Name = "Water Bottle 2L",
            CreatedAt = DateTime.UtcNow
        };
        context.ItemVariants.Add(variant);
        await context.SaveChangesAsync();

        IUserContext userContext = Substitute.For<IUserContext>();
        userContext.UserId.Returns(UserId);
        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);

        var items = new List<PackItemDto> { new(variant.Id, 6) };
        var handler = new CreatePackDefinitionCommandHandler(context, dateTimeProvider, userContext);

        // Act
        Result<Guid> result = await handler.Handle(CreateValidCommand(items: items), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        PackDefinition pack = await context.PackDefinitions
            .Include(p => p.Items)
            .SingleAsync(p => p.Id == result.Value);

        pack.Name.ShouldBe("Starter Pack");
        pack.Items.Count.ShouldBe(1);
        pack.Items.First().ItemVariantId.ShouldBe(variant.Id);
        pack.Items.First().Quantity.ShouldBe(6);
        pack.DomainEvents.ShouldContain(e => e is EntityCreatedDomainEvent<PackDefinition>);
    }

    [Fact]
    public void Validator_Should_HaveValidationError_WhenDuplicateItemVariantsProvided()
    {
        // Arrange
        var validator = new CreatePackDefinitionCommandValidator();
        var duplicateVariantId = Guid.NewGuid();
        CreatePackDefinitionCommand command = CreateValidCommand(items:
        [
            new(duplicateVariantId, 1),
            new(duplicateVariantId, 2)
        ]);

        // Act
        TestValidationResult<CreatePackDefinitionCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items)
            .WithErrorMessage("Pack items cannot contain duplicate item variants.");
    }

    [Fact]
    public void Validator_Should_Pass_WhenItemVariantsAreUnique()
    {
        // Arrange
        var validator = new CreatePackDefinitionCommandValidator();
        CreatePackDefinitionCommand command = CreateValidCommand(items:
        [
            new(Guid.NewGuid(), 1),
            new(Guid.NewGuid(), 2)
        ]);

        // Act
        TestValidationResult<CreatePackDefinitionCommand> result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Items);
    }
}
