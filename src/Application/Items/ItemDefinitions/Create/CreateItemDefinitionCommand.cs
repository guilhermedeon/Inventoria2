using Application.Abstractions.Messaging;

namespace Application.Items.ItemDefinitions.Create;

public sealed record CreateItemDefinitionCommand
(
    Guid UserId,
    string Name,
    string? Description,
    string? Brand,
    string? Manufacturer,
    string? Model,
    string? Sku,
    Guid? CategoryId
) : ICommand<Guid>;
