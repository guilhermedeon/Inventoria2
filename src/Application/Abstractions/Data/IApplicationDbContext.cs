using Domain.Classifications;
using Domain.Inventories;
using Domain.Items;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<RefreshToken> RefreshTokens { get; set; }

    DbSet<TodoItem> TodoItems { get; set; }

    DbSet<Category> Categories { get; set; }

    DbSet<Tag> Tags { get; set; }

    DbSet<Inventory> Inventory { get; set; }

    DbSet<InventoryItem> InventoryItems { get; set; }

    DbSet<ItemDefinition> ItemDefinitions { get; set; }

    DbSet<ItemVariant> ItemVariants { get; set; }

    DbSet<PackDefinition> PackDefinitions { get; set; }

    DbSet<PackDefinitionItem> PackDefinitionItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
