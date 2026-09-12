using Application.Abstractions.Messaging;

namespace Application.Inventories.InventoryItems.Update;

public sealed record UpdateInventoryItemCommand
(
    Guid UserId,
    Guid InventoryItemId,
    Guid InventoryId,
    Guid? ItemVariantId,
    Guid? PackDefinitionId,
    decimal Quantity,
    string? Notes,
    string? SerialNumber,
    string? AssetNumber,
    Guid? CategoryId
) : ICommand;
