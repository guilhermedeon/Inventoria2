using Application.Abstractions.Messaging;

namespace Application.Classifications.Categories.Delete;

public sealed record DeleteCategoryCommand(Guid UserId, Guid CategoryId) : ICommand;
