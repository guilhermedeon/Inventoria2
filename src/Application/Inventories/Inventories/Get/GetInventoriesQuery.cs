using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.GetById;

namespace Application.Inventories.Inventories.Get;

public sealed record GetInventoriesQuery
(
    Guid? ParentInventoryId = null,
    Guid? CategoryId = null
) : IQuery<List<InventoryResponse>>;
