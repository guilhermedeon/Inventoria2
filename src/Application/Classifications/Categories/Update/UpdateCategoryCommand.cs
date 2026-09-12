using Application.Abstractions.Messaging;

namespace Application.Classifications.Categories.Update;

public sealed record UpdateCategoryCommand
(
    Guid UserId,
    Guid CategoryId,
    string Name,
    string Slug,
    Guid? ParentId
) : ICommand;
