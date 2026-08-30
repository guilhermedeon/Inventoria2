using SharedKernel;

namespace Domain.Classifications;

public class Category : Entity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }

    public ICollection<Category> Children { get; set; }
        = [];
}
