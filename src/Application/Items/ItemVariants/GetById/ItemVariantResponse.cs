namespace Application.Items.ItemVariants.GetById;

public sealed class ItemVariantResponse
{
    public Guid Id { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
