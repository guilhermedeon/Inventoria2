using Application.Abstractions.Messaging;
using Application.Inventories.InventoryItems.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InventoryItems;

internal sealed class Create : IEndpoint
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
        app.MapPost("inventory-items", async (
            Request request,
            ICommandHandler<CreateInventoryItemCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateInventoryItemCommand(
                request.UserId,
                request.InventoryId,
                request.ItemVariantId,
                request.PackDefinitionId,
                request.Quantity,
                request.Notes,
                request.SerialNumber,
                request.AssetNumber,
                request.CategoryId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InventoryItems)
        .RequireAuthorization();
    }
}
