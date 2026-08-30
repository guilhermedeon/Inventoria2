using Domain.Classifications;
using Domain.Common;
using Domain.Items;
using SharedKernel;

namespace Domain.Inventories;
/// <summary>
/// Represents something that actually exists in an inventory.
///
/// Examples:
///
/// 2 x Pack of 6 Water Bottle 2L
/// 1 x iPhone 17 256GB Black
/// 50 x M4 Screw
/// </summary>
public class InventoryItem : Entity, ITaggable, ICategorizable
{
    public Guid InventoryId { get; set; }
    public required Inventory Inventory { get; set; }

    /// <summary>
    /// Either a normal item variant OR a pack.
    /// </summary>
    public Guid? ItemVariantId { get; set; }
    public ItemVariant? ItemVariant { get; set; }

    public Guid? PackDefinitionId { get; set; }
    public PackDefinition? PackDefinition { get; set; }

    /// <summary>
    /// Number of units currently present.
    ///
    /// 2 x Pack of 6
    /// => Quantity = 2
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    public string? Notes { get; set; }

    /// <summary>
    /// Optional human-readable identifier.
    /// Useful for things like serial numbers.
    /// </summary>
    public string? SerialNumber { get; set; }

    public string? AssetNumber { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; }
        = [];
}
