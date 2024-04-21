using System.ComponentModel.DataAnnotations;

namespace TestingApi.Helpers.ValidationAttributes;

public class AllowedImageExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".ico"};

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"Invalid file format. Allowed file extensions: " +
                                            $"({string.Join(", ", _allowedExtensions)})");
            }
        }

        return ValidationResult.Success;
    }
}