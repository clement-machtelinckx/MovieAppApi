using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Application.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public class PositiveIntListAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            // [Required] gère le null si nécessaire
            return ValidationResult.Success;
        }

        if (value is IEnumerable<int> ints)
        {
            var list = ints.ToList();

            if (!list.Any())
            {
                return new ValidationResult(
                    ErrorMessage ?? $"{validationContext.DisplayName} must not be empty.");
            }

            if (list.Any(i => i <= 0))
            {
                return new ValidationResult(
                    ErrorMessage ?? $"{validationContext.DisplayName} must contain only positive integers.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"{validationContext.DisplayName} must be a list of integers.");
    }
}
