using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.Inventories.GetById;

internal sealed class GetInventoryByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInventoryByIdQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(GetInventoryByIdQuery query, CancellationToken cancellationToken)
    {
        InventoryResponse? inventory = await context.Inventory
            .AsNoTracking()
            .Where(i => i.Id == query.InventoryId && !i.IsDeleted)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (inventory is null)
        {
            return Result.Failure<InventoryResponse>(InventoryErrors.NotFound(query.InventoryId));
        }

        return inventory;
    }
}
