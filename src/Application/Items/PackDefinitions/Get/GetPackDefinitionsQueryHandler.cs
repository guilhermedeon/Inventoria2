using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions.GetById;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.PackDefinitions.Get;

internal sealed class GetPackDefinitionsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetPackDefinitionsQuery, List<PackDefinitionResponse>>
{
    public async Task<Result<List<PackDefinitionResponse>>> Handle(GetPackDefinitionsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<PackDefinition> queryable = context.PackDefinitions
            .AsNoTracking()
            .Where(p => !p.IsDeleted);

        if (query.CategoryId.HasValue)
        {
            queryable = queryable.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        List<PackDefinitionResponse> packs = await queryable
            .Select(p => new PackDefinitionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Sku = p.Sku,
                Barcode = p.Barcode,
                CategoryId = p.CategoryId,
                Items = p.Items.Select(i => new PackDefinitionItemResponse
                {
                    ItemVariantId = i.ItemVariantId,
                    Quantity = i.Quantity
                }).ToList(),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return packs;
    }
}
