using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.GetById;
using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.InventoryItems.Get;

internal sealed class GetInventoryItemsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInventoryItemsQuery, List<InventoryItemResponse>>
{
    public async Task<Result<List<InventoryItemResponse>>> Handle(GetInventoryItemsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<InventoryItem> queryable = context.InventoryItems
            .AsNoTracking()
            .Where(i => !i.IsDeleted);

        if (query.InventoryId.HasValue)
        {
            queryable = queryable.Where(i => i.InventoryId == query.InventoryId.Value);
        }

        List<InventoryItemResponse> items = await queryable
            .Select(i => new InventoryItemResponse
            {
                Id = i.Id,
                InventoryId = i.InventoryId,
                ItemVariantId = i.ItemVariantId,
                PackDefinitionId = i.PackDefinitionId,
                Quantity = i.Quantity,
                Notes = i.Notes,
                SerialNumber = i.SerialNumber,
                AssetNumber = i.AssetNumber,
                CategoryId = i.CategoryId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}
