using Application.Abstractions.Messaging;

namespace Application.Inventories.InventoryItems.GetById;

public sealed record GetInventoryItemByIdQuery(Guid InventoryItemId) : IQuery<InventoryItemResponse>;
