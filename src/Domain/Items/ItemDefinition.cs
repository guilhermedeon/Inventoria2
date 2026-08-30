using Domain.Classifications;
using Domain.Common;
using SharedKernel;

namespace Domain.Items;

/// <summary>
/// Defines the general kind of thing.
/// Examples: Water Bottle, iPhone, Screw, Backpack.
/// </summary>
public class ItemDefinition : Entity, ITaggable, ICategorizable
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Brand { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? Sku { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Tag> Tags { get; set; }
        = [];

    public ICollection<ItemVariant> Variants { get; set; }
        = [];
}
