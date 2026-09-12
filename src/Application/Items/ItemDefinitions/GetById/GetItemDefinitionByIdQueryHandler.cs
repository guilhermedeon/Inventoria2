using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Items.ItemDefinitions.GetById;

internal sealed class GetItemDefinitionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetItemDefinitionByIdQuery, ItemDefinitionResponse>
{
    public async Task<Result<ItemDefinitionResponse>> Handle(GetItemDefinitionByIdQuery query, CancellationToken cancellationToken)
    {
        ItemDefinitionResponse? item = await context.ItemDefinitions
            .AsNoTracking()
            .Where(i => i.Id == query.ItemDefinitionId && !i.IsDeleted)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (item is null)
        {
            return Result.Failure<ItemDefinitionResponse>(ItemDefinitionErrors.NotFound(query.ItemDefinitionId));
        }

        return item;
    }
}
