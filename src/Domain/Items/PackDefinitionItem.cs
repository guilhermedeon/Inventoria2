namespace Domain.Items;

/// <summary>
/// Contents of a pack.
///
/// Example:
/// Pack of 6 Water Bottle 2L
///
/// ItemVariant = Water Bottle 2L
/// Quantity = 6
/// </summary>
public class PackDefinitionItem
{
    public Guid PackDefinitionId { get; set; }
    public required PackDefinition PackDefinition { get; set; }

    public Guid ItemVariantId { get; set; }
    public required ItemVariant ItemVariant { get; set; }

    public decimal Quantity { get; set; }
}
