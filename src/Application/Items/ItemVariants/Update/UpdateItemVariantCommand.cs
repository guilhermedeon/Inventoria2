using Application.Abstractions.Messaging;

namespace Application.Items.ItemVariants.Update;

public sealed record UpdateItemVariantCommand
(
    Guid UserId,
    Guid ItemVariantId,
    Guid ItemDefinitionId,
    string Name,
    string? Description,
    string? Sku,
    string? Barcode,
    Guid? CategoryId
) : ICommand;
