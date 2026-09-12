using Application.Abstractions.Messaging;

namespace Application.Items.PackDefinitions.Delete;

public sealed record DeletePackDefinitionCommand(Guid UserId, Guid PackDefinitionId) : ICommand;
