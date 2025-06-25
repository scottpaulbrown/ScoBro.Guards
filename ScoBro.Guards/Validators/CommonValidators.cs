namespace ScoBro.Guards;

public record class RequiredMaxLengthStringValidator : Validator<string> {
    public RequiredMaxLengthStringValidator(int maxLength) {
        RuleFor(value => value)
            .IsNotNullOrEmpty(stopIfInvalid: true)
            .HasMaxLength(maxLength);
    }
}