using Application.Abstractions.Messaging;

namespace Application.Items.ItemVariants.GetById;

public sealed record GetItemVariantByIdQuery(Guid ItemVariantId) : IQuery<ItemVariantResponse>;
