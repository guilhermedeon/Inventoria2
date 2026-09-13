using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InventoryItems;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public Guid InventoryId { get; set; }
        public Guid? ItemVariantId { get; set; }
        public Guid? PackDefinitionId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
        public string? SerialNumber { get; set; }
        public string? AssetNumber { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inventory-items/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateInventoryItemCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateInventoryItemCommand(
                request.UserId,
                id,
                request.InventoryId,
                request.ItemVariantId,
                request.PackDefinitionId,
                request.Quantity,
                request.Notes,
                request.SerialNumber,
                request.AssetNumber,
                request.CategoryId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.InventoryItems)
        .RequireAuthorization();
    }
}
