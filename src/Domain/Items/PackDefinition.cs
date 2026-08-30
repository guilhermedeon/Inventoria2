using Domain.Classifications;
using Domain.Common;
using Domain.Inventories;
using SharedKernel;

namespace Domain.Items;

/// <summary>
/// Defines a package/bundle.
/// Example:
/// "6 x Water Bottle 2L"
///
/// A pack is itself a sellable/storable unit, but its
/// contents are defined here.
/// </summary>
public class PackDefinition : Entity, ITaggable, ICategorizable
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Sku { get; set; }

    public string? Barcode { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; }
        = [];

    public ICollection<PackDefinitionItem> Items { get; set; }
        = [];

    public ICollection<InventoryItem> InventoryItems { get; set; }
        = [];
}
