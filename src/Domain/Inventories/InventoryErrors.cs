using SharedKernel;

namespace Domain.Inventories;

public static class InventoryErrors
{
    public static Error NotFound(Guid inventoryId) => Error.NotFound(
        "Inventories.NotFound",
        $"The inventory with the Id = '{inventoryId}' was not found.");

    public static Error CannotBeParentOfItself() => Error.Problem(
        "Inventories.CannotBeParentOfItself",
        "An inventory cannot be its own parent.");

    public static Error ParentNotFound(Guid parentInventoryId) => Error.NotFound(
        "Inventories.ParentNotFound",
        $"The parent inventory with the Id = '{parentInventoryId}' was not found.");
}
