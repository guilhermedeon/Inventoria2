using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.GetById;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemDefinitions.Get;

internal sealed class GetItemDefinitionsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetItemDefinitionsQuery, List<ItemDefinitionResponse>>
{
    public async Task<Result<List<ItemDefinitionResponse>>> Handle(GetItemDefinitionsQuery query, CancellationToken cancellationToken)
    {
        IQueryable<ItemDefinition> queryable = context.ItemDefinitions
            .AsNoTracking()
            .Where(i => !i.IsDeleted);

        if (query.CategoryId.HasValue)
        {
            queryable = queryable.Where(i => i.CategoryId == query.CategoryId.Value);
        }

        List<ItemDefinitionResponse> items = await queryable
            .Select(i => new ItemDefinitionResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Brand = i.Brand,
                Manufacturer = i.Manufacturer,
                Model = i.Model,
                Sku = i.Sku,
                CategoryId = i.CategoryId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}
