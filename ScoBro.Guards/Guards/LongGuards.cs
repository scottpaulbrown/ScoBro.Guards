namespace ScoBro.Guards;
public static class LongGuards {
    public static GuardedValue<long> IsGreaterThan(this GuardedValue<long> guardedValue, long greaterThanValue, string? message = null) {
        if (guardedValue.ValueToValidate <= greaterThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be greater than {greaterThanValue}");

        return guardedValue;
    }

    public static GuardedValue<long> IsGreaterThanOrEqualTo(this GuardedValue<long> guardedValue, long minValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be greater than or equal to {minValue}");

        return guardedValue;
    }

    public static GuardedValue<long> IsLessThan(this GuardedValue<long> guardedValue, long lessThanValue, string? message = null) {
        if (guardedValue.ValueToValidate >= lessThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be less than {lessThanValue}");

        return guardedValue;
    }

    public static GuardedValue<long> IsLessThanOrEqualTo(this GuardedValue<long> guardedValue, long maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be less than or equal to {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<long> IsBetween(this GuardedValue<long> guardedValue, long minValue, long maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue || guardedValue.ValueToValidate > maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be between {minValue} and {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<long> IsNotNegative(this GuardedValue<long> guardedValue, string? message = null) {
        if (guardedValue.ValueToValidate < 0)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} cannot be negative");

        return guardedValue;
    }
}
