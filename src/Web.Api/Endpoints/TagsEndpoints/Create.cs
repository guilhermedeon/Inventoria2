using Application.Abstractions.Messaging;
using Application.Classifications.Tags.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.TagsEndpoints;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("tags", async (
            Request request,
            ICommandHandler<CreateTagCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateTagCommand(
                request.UserId,
                request.Name,
                request.Slug
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TagsTag)
        .RequireAuthorization();
    }
}
