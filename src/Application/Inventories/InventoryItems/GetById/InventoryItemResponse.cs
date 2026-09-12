namespace Application.Inventories.InventoryItems.GetById;

public sealed class InventoryItemResponse
{
    public Guid Id { get; set; }
    public Guid InventoryId { get; set; }
    public Guid? ItemVariantId { get; set; }
    public Guid? PackDefinitionId { get; set; }
    public decimal Quantity { get; set; }
    public string? Notes { get; set; }
    public string? SerialNumber { get; set; }
    public string? AssetNumber { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
