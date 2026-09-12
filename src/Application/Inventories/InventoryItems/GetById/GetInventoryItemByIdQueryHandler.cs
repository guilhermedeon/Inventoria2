using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inventories.InventoryItems.GetById;

internal sealed class GetInventoryItemByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInventoryItemByIdQuery, InventoryItemResponse>
{
    public async Task<Result<InventoryItemResponse>> Handle(GetInventoryItemByIdQuery query, CancellationToken cancellationToken)
    {
        InventoryItemResponse? inventoryItem = await context.InventoryItems
            .AsNoTracking()
            .Where(i => i.Id == query.InventoryItemId && !i.IsDeleted)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (inventoryItem is null)
        {
            return Result.Failure<InventoryItemResponse>(InventoryItemErrors.NotFound(query.InventoryItemId));
        }

        return inventoryItem;
    }
}
