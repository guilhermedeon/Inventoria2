using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InventoryItems;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory-items/{id:guid}", async (
            Guid id,
            IQueryHandler<GetInventoryItemByIdQuery, InventoryItemResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInventoryItemByIdQuery(id);

            Result<InventoryItemResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InventoryItems)
        .RequireAuthorization();
    }
}
