using Application.Abstractions.Messaging;

namespace Application.Classifications.Tags.GetById;

public sealed record GetTagByIdQuery(Guid TagId) : IQuery<TagResponse>;
