using Application.Abstractions.Messaging;

namespace Application.Classifications.Tags.Update;

public sealed record UpdateTagCommand
(
    Guid UserId,
    Guid TagId,
    string Name,
    string Slug
) : ICommand;
