using FluentValidation;

namespace Application.Classifications.Categories.Delete;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
