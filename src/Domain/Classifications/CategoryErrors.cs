using SharedKernel;

namespace Domain.Classifications;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with the Id = '{categoryId}' was not found.");

    public static Error SlugNotUnique(string slug) => Error.Conflict(
        "Categories.SlugNotUnique",
        $"The category slug '{slug}' is already in use.");

    public static Error CannotBeParentOfItself() => Error.Problem(
        "Categories.CannotBeParentOfItself",
        "A category cannot be its own parent.");

    public static Error ParentNotFound(Guid parentId) => Error.NotFound(
        "Categories.ParentNotFound",
        $"The parent category with the Id = '{parentId}' was not found.");
}
