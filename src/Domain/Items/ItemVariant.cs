using Domain.Classifications;
using Domain.Common;
using Domain.Inventories;
using SharedKernel;

namespace Domain.Items;

/// <summary>
/// A specific configuration of an ItemDefinition.
/// Example:
/// Water Bottle -> 2L
/// iPhone -> 256GB / Black
/// </summary>
public class ItemVariant : Entity, ITaggable, ICategorizable
{
    public Guid ItemDefinitionId { get; set; }
    public required ItemDefinition ItemDefinition { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Sku { get; set; }

    public string? Barcode { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; }
        = [];

    public ICollection<PackDefinitionItem> Packs { get; set; }
        = [];

    public ICollection<InventoryItem> InventoryItems { get; set; }
        = [];
}
