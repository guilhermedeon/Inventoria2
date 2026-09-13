using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.Get;
using Application.Items.ItemDefinitions.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemDefinitions;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item-definitions", async (
            IQueryHandler<GetItemDefinitionsQuery, List<ItemDefinitionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetItemDefinitionsQuery();

            Result<List<ItemDefinitionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemDefinitions)
        .RequireAuthorization();
    }
}
