using FluentValidation;

namespace Chapters.Validators;

public class RatingValidator : AbstractValidator<int>
{
    public RatingValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .WithMessage("Rating must be between 1 and 5.");;
    }
}