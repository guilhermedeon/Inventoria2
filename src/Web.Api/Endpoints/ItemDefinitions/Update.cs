using Application.Abstractions.Messaging;
using Application.Items.ItemDefinitions.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ItemDefinitions;

internal sealed class Update : IEndpoint
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
        app.MapPut("item-definitions/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateItemDefinitionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateItemDefinitionCommand(
                request.UserId,
                id,
                request.Name,
                request.Description,
                request.Brand,
                request.Manufacturer,
                request.Model,
                request.Sku,
                request.CategoryId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.ItemDefinitions)
        .RequireAuthorization();
    }
}
