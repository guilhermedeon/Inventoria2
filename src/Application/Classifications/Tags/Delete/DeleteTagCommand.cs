using Application.Abstractions.Messaging;

namespace Application.Classifications.Tags.Delete;

public sealed record DeleteTagCommand(Guid UserId, Guid TagId) : ICommand;
