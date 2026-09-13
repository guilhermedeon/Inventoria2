using Application.Abstractions.Messaging;
using Application.Classifications.Categories.Get;
using Application.Classifications.Categories.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Categories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
            IQueryHandler<GetCategoriesQuery, List<CategoryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCategoriesQuery();

            Result<List<CategoryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Categories)
        .RequireAuthorization();
    }
}
