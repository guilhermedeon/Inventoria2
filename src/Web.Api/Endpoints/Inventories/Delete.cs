using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inventories;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("inventories/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeleteInventoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInventoryCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
