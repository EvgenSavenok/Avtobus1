using Avtobus1.Domain.Entities;
using FluentValidation;

namespace Avtobus1.Application.Validation;

public class UrlValidator : AbstractValidator<UrlRecord>
{
    public UrlValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty().WithMessage("Original URL is required.")
            .MaximumLength(2048).WithMessage("URL must not exceed 2048 characters.") 
            .Must(IsValidUrl).WithMessage("Original URL must be a valid HTTP or HTTPS address.");

        RuleFor(x => x.ShortCode)
            .NotEmpty().WithMessage("Short Code is required.")
            .MaximumLength(10).WithMessage("Short Code length must not exceed 10 characters.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Short Code can only contain letters and numbers.");

        // RuleFor(x => x.CreatedAt)
        //     .NotEmpty().WithMessage("CreatedAt is required.")
        //     .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

        RuleFor(x => x.ClickCount)
            .GreaterThanOrEqualTo(0).WithMessage("Click count cannot be negative.");
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}