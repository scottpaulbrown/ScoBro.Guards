namespace ScoBro.Guards;

public record class ValidatingValue<T>(T ValueToValidate, string FieldName) {
    public List<string> Errors { get; } = [];
}