using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.Get;
using Application.Items.ItemVariants.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemVariants;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item-variants", async (
            IQueryHandler<GetItemVariantsQuery, List<ItemVariantResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetItemVariantsQuery();

            Result<List<ItemVariantResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemVariants)
        .RequireAuthorization();
    }
}
