using Application.Abstractions.Messaging;
using Application.Classifications.Tags.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.TagsEndpoints;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("tags/{id:guid}", async (
            Guid id,
            Guid userId,
            ICommandHandler<DeleteTagCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTagCommand(userId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.TagsTag)
        .RequireAuthorization();
    }
}
