using Application.Abstractions.Messaging;

namespace Application.Items.ItemDefinitions.Update;

public sealed record UpdateItemDefinitionCommand
(
    Guid UserId,
    Guid ItemDefinitionId,
    string Name,
    string? Description,
    string? Brand,
    string? Manufacturer,
    string? Model,
    string? Sku,
    Guid? CategoryId
) : ICommand;
