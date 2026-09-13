using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemVariants;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("item-variants", async (
            Request request,
            ICommandHandler<CreateItemVariantCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateItemVariantCommand(
                request.UserId,
                request.ItemDefinitionId,
                request.Name,
                request.Description,
                request.Sku,
                request.Barcode,
                request.CategoryId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemVariants)
        .RequireAuthorization();
    }
}
