using Application.Abstractions.Messaging;

namespace Application.Classifications.Categories.Create;

public sealed record CreateCategoryCommand
(
    Guid UserId,
    string Name,
    string Slug,
    Guid? ParentId
) : ICommand<Guid>;
