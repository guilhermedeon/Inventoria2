using Application.Abstractions.Messaging;

namespace Application.Inventories.InventoryItems.Delete;

public sealed record DeleteInventoryItemCommand(Guid UserId, Guid InventoryItemId) : ICommand;
