using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Classifications.Categories.GetById;
using Domain.Classifications;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Categories.Get;

internal sealed class GetCategoriesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Category> queryable = context.Categories
            .AsNoTracking()
            .Where(c => !c.IsDeleted);

        if (query.ParentId.HasValue)
        {
            queryable = queryable.Where(c => c.ParentId == query.ParentId.Value);
        }

        List<CategoryResponse> categories = await queryable
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                ParentId = c.ParentId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return categories;
    }
}
