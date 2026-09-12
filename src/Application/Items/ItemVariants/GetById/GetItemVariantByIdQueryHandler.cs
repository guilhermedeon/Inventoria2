using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemVariants.GetById;

internal sealed class GetItemVariantByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetItemVariantByIdQuery, ItemVariantResponse>
{
    public async Task<Result<ItemVariantResponse>> Handle(GetItemVariantByIdQuery query, CancellationToken cancellationToken)
    {
        ItemVariantResponse? variant = await context.ItemVariants
            .AsNoTracking()
            .Where(v => v.Id == query.ItemVariantId && !v.IsDeleted)
            .Select(v => new ItemVariantResponse
            {
                Id = v.Id,
                ItemDefinitionId = v.ItemDefinitionId,
                Name = v.Name,
                Description = v.Description,
                Sku = v.Sku,
                Barcode = v.Barcode,
                CategoryId = v.CategoryId,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (variant is null)
        {
            return Result.Failure<ItemVariantResponse>(ItemVariantErrors.NotFound(query.ItemVariantId));
        }

        return variant;
    }
}
