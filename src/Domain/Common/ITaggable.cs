using Domain.Classifications;

namespace Domain.Common;

public interface ITaggable
{
    ICollection<Tag> Tags { get; }
}
