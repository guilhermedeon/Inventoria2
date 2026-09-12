using Application.Abstractions.Messaging;

namespace Application.Items.ItemDefinitions.GetById;

public sealed record GetItemDefinitionByIdQuery(Guid ItemDefinitionId) : IQuery<ItemDefinitionResponse>;
