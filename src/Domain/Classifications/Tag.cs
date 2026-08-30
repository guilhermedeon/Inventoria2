using SharedKernel;

namespace Domain.Classifications;

public class Tag : Entity
{
    public required string Name { get; set; }

    // Useful for normalized searches / uniqueness.
    public required string Slug { get; set; }

    public ICollection<TagAssignment> Assignments { get; set; }
        = [];
}
