using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Application.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public class AllowedStringValuesAttribute : ValidationAttribute
{
    private readonly string[] _allowed;

    public AllowedStringValuesAttribute(params string[] allowed)
    {
        _allowed = allowed ?? Array.Empty<string>();
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            // [Required] gère le null, donc ici on considère ça comme "ok"
            return ValidationResult.Success;
        }

        if (value is string s)
        {
            if (_allowed.Contains(s))
            {
                return ValidationResult.Success;
            }

            var msg = ErrorMessage ??
                      $"The field {validationContext.DisplayName} must be one of: {string.Join(", ", _allowed)}.";
            return new ValidationResult(msg);
        }

        return new ValidationResult($"The field {validationContext.DisplayName} must be a string.");
    }
}
