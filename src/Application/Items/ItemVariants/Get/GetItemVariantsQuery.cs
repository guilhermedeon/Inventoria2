using Application.Abstractions.Messaging;
using Application.Items.ItemVariants.GetById;

namespace Application.Items.ItemVariants.Get;

public sealed record GetItemVariantsQuery(Guid? ItemDefinitionId = null) : IQuery<List<ItemVariantResponse>>;
