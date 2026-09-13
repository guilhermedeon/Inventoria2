using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemVariants;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("item-variants/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeleteItemVariantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteItemVariantCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.ItemVariants)
        .RequireAuthorization();
    }
}
