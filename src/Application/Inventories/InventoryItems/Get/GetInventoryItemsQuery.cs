using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.GetById;

namespace Application.Inventories.InventoryItems.Get;

public sealed record GetInventoryItemsQuery(Guid? InventoryId = null) : IQuery<List<InventoryItemResponse>>;
