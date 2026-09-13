using Application.Abstractions.Messaging;
using Application.Inventories.Inventories.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inventories;

internal sealed class Create : IEndpoint
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
        app.MapPost("inventories", async (
            Request request,
            ICommandHandler<CreateInventoryCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateInventoryCommand(
                request.UserId,
                request.Name,
                request.Description,
                request.CategoryId,
                request.ParentInventoryId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
