namespace Domain.Classifications;

public class TagAssignment
{
    public Guid TagId { get; set; }
    public required Tag Tag { get; set; }

    public Guid EntityId { get; set; }

    // Identifies what kind of entity EntityId refers to.
    public required TagEntityType EntityType { get; set; }
}
