using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions.GetById;

namespace Application.Items.PackDefinitions.Get;

public sealed record GetPackDefinitionsQuery(Guid? CategoryId = null) : IQuery<List<PackDefinitionResponse>>;
