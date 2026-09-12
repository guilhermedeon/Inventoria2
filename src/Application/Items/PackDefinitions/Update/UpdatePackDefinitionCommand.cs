using Application.Abstractions.Messaging;

namespace Application.Items.PackDefinitions.Update;

public sealed record UpdatePackDefinitionCommand
(
    Guid UserId,
    Guid PackDefinitionId,
    string Name,
    string? Description,
    string? Sku,
    string? Barcode,
    Guid? CategoryId,
    List<PackItemDto>? Items = null
) : ICommand;
