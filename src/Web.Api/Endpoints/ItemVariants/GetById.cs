using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemVariants;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("item-variants/{id:guid}", async (
            Guid id,
            IQueryHandler<GetItemVariantByIdQuery, ItemVariantResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetItemVariantByIdQuery(id);

            Result<ItemVariantResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemVariants)
        .RequireAuthorization();
    }
}
