using FluentValidation;

namespace Application.Classifications.Tags.Delete;

public sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
