using System.ComponentModel.DataAnnotations;

namespace TestingApi.Helpers.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class EnumValueAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValueAttribute(Type enumType)
    {
        if (enumType is null || !enumType.IsEnum)
        {
            throw new ArgumentException("The provided type is not an enum.", nameof(enumType));
        }

        _enumType = enumType;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;
        
        var success = Enum.TryParse(_enumType, value?.ToString(), true, out var parsed);

        return success
            ? ValidationResult.Success
            : new ValidationResult($"{value} is not a valid {_enumType.Name} value.");
    }
}