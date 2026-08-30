using Domain.Classifications;

namespace Domain.Common;

public interface ICategorizable
{
    Guid? CategoryId { get; set; }
    Category? Category { get; set; }
}
