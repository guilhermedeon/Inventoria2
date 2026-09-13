using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions.Get;
using Application.Items.PackDefinitions.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PackDefinitions;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("pack-definitions", async (
            IQueryHandler<GetPackDefinitionsQuery, List<PackDefinitionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPackDefinitionsQuery();

            Result<List<PackDefinitionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.PackDefinitions)
        .RequireAuthorization();
    }
}
