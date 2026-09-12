using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.GetById;
using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.Inventories.Get;

internal sealed class GetInventoriesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInventoriesQuery, List<InventoryResponse>>
{
    public async Task<Result<List<InventoryResponse>>> Handle(GetInventoriesQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Inventory> queryable = context.Inventory
            .AsNoTracking()
            .Where(i => !i.IsDeleted);

        if (query.ParentInventoryId.HasValue)
        {
            queryable = queryable.Where(i => i.ParentInventoryId == query.ParentInventoryId.Value);
        }

        if (query.CategoryId.HasValue)
        {
            queryable = queryable.Where(i => i.CategoryId == query.CategoryId.Value);
        }

        List<InventoryResponse> inventories = await queryable
            .Select(i => new InventoryResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                CategoryId = i.CategoryId,
                ParentInventoryId = i.ParentInventoryId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return inventories;
    }
}
