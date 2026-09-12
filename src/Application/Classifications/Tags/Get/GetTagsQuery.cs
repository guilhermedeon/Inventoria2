using Application.Abstractions.Messaging;
using Application.Classifications.Tags.GetById;

namespace Application.Classifications.Tags.Get;

public sealed record GetTagsQuery() : IQuery<List<TagResponse>>;
