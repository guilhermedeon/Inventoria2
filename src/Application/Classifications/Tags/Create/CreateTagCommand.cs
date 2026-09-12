using Application.Abstractions.Messaging;

namespace Application.Classifications.Tags.Create;

public sealed record CreateTagCommand
(
    Guid UserId,
    string Name,
    string Slug
) : ICommand<Guid>;
