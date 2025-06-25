namespace ScoBro.Guards;

public static class GeneralGuards {
    public static GuardedValue<T> IsNotNull<T>(this GuardedValue<T> guardedValue, string? message = null) {
        if (guardedValue.ValueToValidate == null)
            throw new ArgumentNullException(guardedValue.FieldName, message);
        return guardedValue;
    }

    public static T ObjectIsNotNull<T>(this GuardedValue<T> guard) =>
        guard.IsNotNull().ValueToValidate;

    public static GuardedValue<T> IsNotTrue<T>(this GuardedValue<T> guardedValue, Func<T, bool> criteria, string? message = null) {
        if (criteria(guardedValue.ValueToValidate))
            throw new ArgumentException(message, guardedValue.FieldName);
        return guardedValue;
    }
}
