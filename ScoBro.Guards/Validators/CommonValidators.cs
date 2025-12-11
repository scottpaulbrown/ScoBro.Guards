using ScoBro.Foundation;

namespace ScoBro.Guards;

public record class RequiredMaxLengthStringValidator : Validator<string> {
    public RequiredMaxLengthStringValidator(int maxLength, string? fieldName = null) {
        RuleFor(value => value, fieldName)
            .IsNotNullOrEmpty()
            .HasMaxLength(maxLength);
    }

    public static RequiredMaxLengthStringValidator Create(
        int maxLength, string? fieldName = null) => new(maxLength, fieldName);
}

public record class OptionalMaxLengthStringValidator : Validator<string> {
    public OptionalMaxLengthStringValidator(int maxLength, string? fieldName = null) {
        RuleFor(value => value, fieldName)
            .Optional()
            .HasMaxLength(maxLength);
    }

    public static OptionalMaxLengthStringValidator Create(
        int maxLength, string? fieldName = null) => new(maxLength, fieldName);
}

public record class RequiredDomainEnumValidator<TEnum> : Validator<string> where TEnum : EnumBase<TEnum> {
    public RequiredDomainEnumValidator(string? fieldName = null) {
        RuleFor(value => value, fieldName)
            .IsNotNullOrEmpty()
            .Must(value => EnumBase<TEnum>.TryParse(value, out var _), $"Invalid value for domain enum {typeof(TEnum).Name}");
    }
}