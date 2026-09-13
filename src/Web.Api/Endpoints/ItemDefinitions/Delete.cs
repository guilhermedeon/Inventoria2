using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemDefinitions;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("item-definitions/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeleteItemDefinitionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteItemDefinitionCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.ItemDefinitions)
        .RequireAuthorization();
    }
}
