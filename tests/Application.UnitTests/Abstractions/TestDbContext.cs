using Application.Abstractions.Data;
using Domain.Classifications;
using Domain.Inventories;
using Domain.Items;
using Domain.Todos;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitTests.Abstractions;

/// <summary>
/// A lightweight in-memory <see cref="DbContext"/> that implements <see cref="IApplicationDbContext"/>
/// so Application handlers can be unit tested without referencing the Infrastructure layer.
/// </summary>
public sealed class TestDbContext(DbContextOptions<TestDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<ItemDefinition> ItemDefinitions { get; set; }
    public DbSet<ItemVariant> ItemVariants { get; set; }
    public DbSet<PackDefinition> PackDefinitions { get; set; }
    public DbSet<PackDefinitionItem> PackDefinitionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
