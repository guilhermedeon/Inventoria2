using Application.Abstractions.Messaging;
using Application.Items.PackDefinitions.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.PackDefinitions;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("pack-definitions/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeletePackDefinitionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeletePackDefinitionCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.PackDefinitions)
        .RequireAuthorization();
    }
}
