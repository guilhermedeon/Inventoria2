using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions;
using Application.Items.PackDefinitions.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PackDefinitions;

internal sealed class Create : IEndpoint
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
        app.MapPost("pack-definitions", async (
            Request request,
            ICommandHandler<CreatePackDefinitionCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreatePackDefinitionCommand(
                request.UserId,
                request.Name,
                request.Description,
                request.Sku,
                request.Barcode,
                request.CategoryId,
                [.. request.Items.Select(i => new PackItemDto(i.ItemVariantId, i.Quantity))]
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.PackDefinitions)
        .RequireAuthorization();
    }
}
