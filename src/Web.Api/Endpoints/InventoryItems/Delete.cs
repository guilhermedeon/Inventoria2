using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InventoryItems;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("inventory-items/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeleteInventoryItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInventoryItemCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.InventoryItems)
        .RequireAuthorization();
    }
}
