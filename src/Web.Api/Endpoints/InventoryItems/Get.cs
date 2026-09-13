using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.Get;
using Application.Inventories.InventoryItems.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InventoryItems;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory-items", async (
            IQueryHandler<GetInventoryItemsQuery, List<InventoryItemResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInventoryItemsQuery();

            Result<List<InventoryItemResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InventoryItems)
        .RequireAuthorization();
    }
}
