using Application.Abstractions.Messaging;
using Application.Classifications.Tags.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.TagsEndpoints;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tags/{id:guid}", async (
            Guid id,
            IQueryHandler<GetTagByIdQuery, TagResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTagByIdQuery(id);

            Result<TagResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TagsTag)
        .RequireAuthorization();
    }
}
