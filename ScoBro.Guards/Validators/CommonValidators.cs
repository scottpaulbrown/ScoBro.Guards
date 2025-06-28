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