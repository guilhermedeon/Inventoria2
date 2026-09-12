using Application.Abstractions.Messaging;

namespace Application.Items.ItemDefinitions.Delete;

public sealed record DeleteItemDefinitionCommand(Guid UserId, Guid ItemDefinitionId) : ICommand;
