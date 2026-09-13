using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemVariants;

internal sealed class Update : IEndpoint
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
        app.MapPut("item-variants/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateItemVariantCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateItemVariantCommand(
                request.UserId,
                id,
                request.ItemDefinitionId,
                request.Name,
                request.Description,
                request.Sku,
                request.Barcode,
                request.CategoryId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.ItemVariants)
        .RequireAuthorization();
    }
}
