namespace ScoBro.Guards;
public static class Guard {
    public static GuardedValue<T> For<T>(T valueToValidate, string? fieldName = null) =>
        new(valueToValidate, fieldName ?? nameof(valueToValidate));
}
