using Application.Abstractions.Messaging;
using Application.Classifications.Tags.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.TagsEndpoints;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("tags/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateTagCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTagCommand(
                request.UserId,
                id,
                request.Name,
                request.Slug
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.TagsTag)
        .RequireAuthorization();
    }
}
