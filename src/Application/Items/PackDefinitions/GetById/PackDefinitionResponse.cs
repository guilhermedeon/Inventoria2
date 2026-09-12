namespace Application.Items.PackDefinitions.GetById;

public sealed class PackDefinitionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public Guid? CategoryId { get; set; }
    public List<PackDefinitionItemResponse> Items { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class PackDefinitionItemResponse
{
    public Guid ItemVariantId { get; set; }
    public decimal Quantity { get; set; }
}
