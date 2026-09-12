using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.PackDefinitions.GetById;

internal sealed class GetPackDefinitionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetPackDefinitionByIdQuery, PackDefinitionResponse>
{
    public async Task<Result<PackDefinitionResponse>> Handle(GetPackDefinitionByIdQuery query, CancellationToken cancellationToken)
    {
        PackDefinitionResponse? pack = await context.PackDefinitions
            .AsNoTracking()
            .Where(p => p.Id == query.PackDefinitionId && !p.IsDeleted)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (pack is null)
        {
            return Result.Failure<PackDefinitionResponse>(PackDefinitionErrors.NotFound(query.PackDefinitionId));
        }

        return pack;
    }
}
