using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.GetById;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemVariants.Get;

internal sealed class GetItemVariantsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetItemVariantsQuery, List<ItemVariantResponse>>
{
    public async Task<Result<List<ItemVariantResponse>>> Handle(GetItemVariantsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<ItemVariant> queryable = context.ItemVariants
            .AsNoTracking()
            .Where(v => !v.IsDeleted);

        if (query.ItemDefinitionId.HasValue)
        {
            queryable = queryable.Where(v => v.ItemDefinitionId == query.ItemDefinitionId.Value);
        }

        List<ItemVariantResponse> variants = await queryable
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
            .ToListAsync(cancellationToken);

        return variants;
    }
}
