using Application.Abstractions.Messaging;

namespace Application.Items.PackDefinitions.Create;

public sealed record CreatePackDefinitionCommand
(
    Guid UserId,
    string Name,
    string? Description,
    string? Sku,
    string? Barcode,
    Guid? CategoryId,
    List<PackItemDto>? Items = null
) : ICommand<Guid>;
