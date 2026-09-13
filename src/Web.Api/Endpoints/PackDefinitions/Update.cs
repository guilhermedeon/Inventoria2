using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions;
using Application.Items.PackDefinitions.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PackDefinitions;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public Guid? CategoryId { get; set; }
        public List<PackItemRequest> Items { get; set; } = [];
    }

    public sealed class PackItemRequest
    {
        public Guid ItemVariantId { get; set; }
        public decimal Quantity { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("pack-definitions/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdatePackDefinitionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePackDefinitionCommand(
                request.UserId,
                id,
                request.Name,
                request.Description,
                request.Sku,
                request.Barcode,
                request.CategoryId,
                request.Items.Select(i => new PackItemDto
                {
                    ItemVariantId = i.ItemVariantId,
                    Quantity = i.Quantity
                }).ToList()
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.PackDefinitions)
        .RequireAuthorization();
    }
}
