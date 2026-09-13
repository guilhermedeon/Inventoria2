using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PackDefinitions;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("pack-definitions/{id:guid}", async (
            Guid id,
            IQueryHandler<GetPackDefinitionByIdQuery, PackDefinitionResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPackDefinitionByIdQuery(id);

            Result<PackDefinitionResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.PackDefinitions)
        .RequireAuthorization();
    }
}
