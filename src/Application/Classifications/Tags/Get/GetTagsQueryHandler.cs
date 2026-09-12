using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Classifications.Tags.GetById;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Tags.Get;

internal sealed class GetTagsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTagsQuery, List<TagResponse>>
{
    public async Task<Result<List<TagResponse>>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        List<TagResponse> tags = await context.Tags
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Select(t => new TagResponse
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return tags;
    }
}
