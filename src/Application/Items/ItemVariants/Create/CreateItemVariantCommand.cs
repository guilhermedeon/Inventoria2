using Application.Abstractions.Messaging;

namespace Application.Items.ItemVariants.Create;

public sealed record CreateItemVariantCommand
(
    Guid UserId,
    Guid ItemDefinitionId,
    string Name,
    string? Description,
    string? Sku,
    string? Barcode,
    Guid? CategoryId
) : ICommand<Guid>;
