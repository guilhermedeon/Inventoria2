using Application.Abstractions.Messaging;

namespace Application.Inventories.Inventories.Create;

public sealed record CreateInventoryCommand
(
    Guid UserId,
    string Name,
    string? Description,
    Guid? CategoryId,
    Guid? ParentInventoryId
) : ICommand<Guid>;
