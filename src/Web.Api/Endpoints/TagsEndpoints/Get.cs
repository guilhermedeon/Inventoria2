using Application.Abstractions.Messaging;
using Application.Classifications.Tags.Get;
using Application.Classifications.Tags.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.TagsEndpoints;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tags", async (
            IQueryHandler<GetTagsQuery, List<TagResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTagsQuery();

            Result<List<TagResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TagsTag)
        .RequireAuthorization();
    }
}
