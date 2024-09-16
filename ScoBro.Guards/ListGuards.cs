namespace ScoBro.Guards;

public static class ListGuards {
    public static GuardedValue<List<T>> HasMinimumCount<T>(this GuardedValue<List<T>> guardedValue, int minCount, string? message = null) {
        if (guardedValue.ValueToValidate == null)
            throw new ArgumentNullException(guardedValue.FieldName, message);
        if (guardedValue.ValueToValidate.Count < minCount)
            throw new ArgumentException(message ?? $"{guardedValue.FieldName} must have at least {minCount} elements", guardedValue.FieldName);
        return guardedValue;
    }
}
