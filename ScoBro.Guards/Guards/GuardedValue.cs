
namespace ScoBro.Guards;

public record GuardedValue<T>(T ValueToValidate, string FieldName) {
    public bool HasValue() => ValueToValidate != null;

    public static implicit operator T(GuardedValue<T> value) => value.ValueToValidate;
}
