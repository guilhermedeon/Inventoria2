using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemDefinitions;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item-definitions/{id:guid}", async (
            Guid id,
            IQueryHandler<GetItemDefinitionByIdQuery, ItemDefinitionResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetItemDefinitionByIdQuery(id);

            Result<ItemDefinitionResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemDefinitions)
        .RequireAuthorization();
    }
}
