using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inventories;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventories/{id:guid}", async (
            Guid id,
            IQueryHandler<GetInventoryByIdQuery, InventoryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInventoryByIdQuery(id);

            Result<InventoryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
