using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Classifications;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Categories.GetById;

internal sealed class GetCategoryByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        CategoryResponse? category = await context.Categories
            .AsNoTracking()
            .Where(c => c.Id == query.CategoryId && !c.IsDeleted)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentId = c.ParentId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(query.CategoryId));
        }

        return category;
    }
}
