using Application.Abstractions.Messaging;

namespace Application.Inventories.Inventories.Delete;

public sealed record DeleteInventoryCommand(Guid UserId, Guid InventoryId) : ICommand;
