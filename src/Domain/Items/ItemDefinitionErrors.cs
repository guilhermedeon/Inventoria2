using SharedKernel;

namespace Domain.Items;

public static class ItemDefinitionErrors
{
    public static Error NotFound(Guid itemDefinitionId) => Error.NotFound(
        "ItemDefinitions.NotFound",
        $"The item definition with the Id = '{itemDefinitionId}' was not found.");

    public static Error SkuNotUnique(string sku) => Error.Conflict(
        "ItemDefinitions.SkuNotUnique",
        $"The SKU '{sku}' is already in use.");
}
