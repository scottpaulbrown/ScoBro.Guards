namespace ScoBro.Guards;

public record class ValidatorError(string ErrorMessage, string? FieldName = null) {
    public override string ToString() => ErrorMessage;
}