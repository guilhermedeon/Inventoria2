using SharedKernel;

namespace Domain.Classifications;

public static class TagErrors
{
    public static Error NotFound(Guid tagId) => Error.NotFound(
        "Tags.NotFound",
        $"The tag with the Id = '{tagId}' was not found.");

    public static Error SlugNotUnique(string slug) => Error.Conflict(
        "Tags.SlugNotUnique",
        $"The tag slug '{slug}' is already in use.");
}
