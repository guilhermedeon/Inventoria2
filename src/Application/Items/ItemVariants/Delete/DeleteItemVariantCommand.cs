using Application.Abstractions.Messaging;

namespace Application.Items.ItemVariants.Delete;

public sealed record DeleteItemVariantCommand(Guid UserId, Guid ItemVariantId) : ICommand;
