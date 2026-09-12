using Application.Abstractions.Messaging;

namespace Application.Inventories.Inventories.GetById;

public sealed record GetInventoryByIdQuery(Guid InventoryId) : IQuery<InventoryResponse>;
