namespace ScoBro.Guards;

public record class ValidationResult {
    private readonly List<ValidatorError> _errors;

    public bool IsValid { get; }
    public IReadOnlyList<ValidatorError> Errors => _errors;

    public ValidationResult(bool isValid, List<ValidatorError>? errors = null) {
        IsValid = isValid;
        _errors = errors ?? [];
    }

    public static ValidationResult Valid() => new(true);
    public static ValidationResult Invalid(List<ValidatorError> errors) => new(false, errors);
    public static ValidationResult Invalid(ValidatorError error) => new(false, [error]);
    public static ValidationResult Invalid(string errorMessage, string fieldName = "") =>
        new(false, [new ValidatorError(errorMessage, fieldName)]);
}