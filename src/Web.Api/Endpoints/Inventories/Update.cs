using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inventories;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ParentInventoryId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inventories/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateInventoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateInventoryCommand(
                request.UserId,
                id,
                request.Name,
                request.Description,
                request.CategoryId,
                request.ParentInventoryId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
