using Application.Abstractions.Messaging;

namespace Application.Items.PackDefinitions.GetById;

public sealed record GetPackDefinitionByIdQuery(Guid PackDefinitionId) : IQuery<PackDefinitionResponse>;
