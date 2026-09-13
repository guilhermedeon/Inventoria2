using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemDefinitions;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Sku { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("item-definitions", async (
            Request request,
            ICommandHandler<CreateItemDefinitionCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateItemDefinitionCommand(
                request.UserId,
                request.Name,
                request.Description,
                request.Brand,
                request.Manufacturer,
                request.Model,
                request.Sku,
                request.CategoryId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ItemDefinitions)
        .RequireAuthorization();
    }
}
