using SharedKernel;

namespace Domain.Items;

public static class PackDefinitionErrors
{
    public static Error NotFound(Guid packDefinitionId) => Error.NotFound(
        "PackDefinitions.NotFound",
        $"The pack definition with the Id = '{packDefinitionId}' was not found.");

    public static Error SkuNotUnique(string sku) => Error.Conflict(
        "PackDefinitions.SkuNotUnique",
        $"The SKU '{sku}' is already in use.");

    public static Error ItemVariantNotFound(Guid itemVariantId) => Error.NotFound(
        "PackDefinitions.ItemVariantNotFound",
        $"The item variant with the Id = '{itemVariantId}' was not found.");
}
