using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Classifications;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Classifications.Tags.GetById;

internal sealed class GetTagByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTagByIdQuery, TagResponse>
{
    public async Task<Result<TagResponse>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
    {
        TagResponse? tag = await context.Tags
            .AsNoTracking()
            .Where(t => t.Id == query.TagId && !t.IsDeleted)
            .Select(t => new TagResponse
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            return Result.Failure<TagResponse>(TagErrors.NotFound(query.TagId));
        }

        return tag;
    }
}
