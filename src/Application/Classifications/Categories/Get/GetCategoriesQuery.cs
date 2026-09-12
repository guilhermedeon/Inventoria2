using Application.Abstractions.Messaging;
using Application.Classifications.Categories.GetById;

namespace Application.Classifications.Categories.Get;

public sealed record GetCategoriesQuery(Guid? ParentId = null) : IQuery<List<CategoryResponse>>;
