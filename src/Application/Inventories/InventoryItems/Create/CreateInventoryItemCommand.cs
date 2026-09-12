using Application.Abstractions.Messaging;

namespace Application.Inventories.InventoryItems.Create;

public sealed record CreateInventoryItemCommand
(
    Guid UserId,
    Guid InventoryId,
    Guid? ItemVariantId,
    Guid? PackDefinitionId,
    decimal Quantity,
    string? Notes,
    string? SerialNumber,
    string? AssetNumber,
    Guid? CategoryId
) : ICommand<Guid>;
