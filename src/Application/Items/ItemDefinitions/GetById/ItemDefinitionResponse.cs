namespace Application.Items.ItemDefinitions.GetById;

public sealed class ItemDefinitionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Brand { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Sku { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
