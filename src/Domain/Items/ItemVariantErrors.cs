using SharedKernel;

namespace Domain.Items;

public static class ItemVariantErrors
{
    public static Error NotFound(Guid itemVariantId) => Error.NotFound(
        "ItemVariants.NotFound",
        $"The item variant with the Id = '{itemVariantId}' was not found.");

    public static Error ItemDefinitionNotFound(Guid itemDefinitionId) => Error.NotFound(
        "ItemVariants.ItemDefinitionNotFound",
        $"The item definition with the Id = '{itemDefinitionId}' was not found.");

    public static Error SkuNotUnique(string sku) => Error.Conflict(
        "ItemVariants.SkuNotUnique",
        $"The SKU '{sku}' is already in use.");
}
