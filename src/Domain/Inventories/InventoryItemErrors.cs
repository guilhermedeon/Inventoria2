using SharedKernel;

namespace Domain.Inventories;

public static class InventoryItemErrors
{
    public static Error NotFound(Guid inventoryItemId) => Error.NotFound(
        "InventoryItems.NotFound",
        $"The inventory item with the Id = '{inventoryItemId}' was not found.");

    public static Error InventoryNotFound(Guid inventoryId) => Error.NotFound(
        "InventoryItems.InventoryNotFound",
        $"The inventory with the Id = '{inventoryId}' was not found.");

    public static Error VariantOrPackRequired() => Error.Problem(
        "InventoryItems.VariantOrPackRequired",
        "Either an item variant or a pack definition must be specified, but not both.");

    public static Error ItemVariantNotFound(Guid itemVariantId) => Error.NotFound(
        "InventoryItems.ItemVariantNotFound",
        $"The item variant with the Id = '{itemVariantId}' was not found.");

    public static Error PackDefinitionNotFound(Guid packDefinitionId) => Error.NotFound(
        "InventoryItems.PackDefinitionNotFound",
        $"The pack definition with the Id = '{packDefinitionId}' was not found.");
}
