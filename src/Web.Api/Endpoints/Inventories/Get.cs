using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.Get;
using Application.Inventories.Inventories.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inventories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventories", async (
            IQueryHandler<GetInventoriesQuery, List<InventoryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInventoriesQuery();

            Result<List<InventoryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
