using Application.Abstractions.Messaging;

namespace Application.Inventories.Inventories.Update;

public sealed record UpdateInventoryCommand
(
    Guid UserId,
    Guid InventoryId,
    string Name,
    string? Description,
    Guid? CategoryId,
    Guid? ParentInventoryId
) : ICommand;
