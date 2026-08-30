using Domain.Classifications;
using Domain.Common;
using SharedKernel;

namespace Domain.Inventories;

/// <summary>
/// A container/location capable of containing InventoryItems.
///
/// Examples:
/// House
/// Backpack
/// Box
/// Car
/// Warehouse
/// Drawer
/// </summary>
public class Inventory : Entity, ITaggable, ICategorizable
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; }
        = [];

    public ICollection<InventoryItem> Items { get; set; }
        = [];

    /// <summary>
    /// Allows inventories to contain other inventories.
    /// </summary>
    public Guid? ParentInventoryId { get; set; }
    public Inventory? ParentInventory { get; set; }

    public virtual ICollection<Inventory> ChildInventories { get; set; }
        = [];
}
