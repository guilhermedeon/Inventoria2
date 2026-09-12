using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.GetById;

namespace Application.Items.ItemDefinitions.Get;

public sealed record GetItemDefinitionsQuery(Guid? CategoryId = null) : IQuery<List<ItemDefinitionResponse>>;
