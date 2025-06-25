namespace ScoBro.Guards;
public static class DoubleGuards {
    public static GuardedValue<double> IsGreaterThan(this GuardedValue<double> guardedValue, double greaterThanValue, string? message = null) {
        if (guardedValue.ValueToValidate <= greaterThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be greater than {greaterThanValue}");

        return guardedValue;
    }

    public static GuardedValue<double> IsGreaterThanOrEqualTo(this GuardedValue<double> guardedValue, double minValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be greater than or equal to {minValue}");

        return guardedValue;
    }

    public static GuardedValue<double> IsLessThan(this GuardedValue<double> guardedValue, double lessThanValue, string? message = null) {
        if (guardedValue.ValueToValidate >= lessThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be less than {lessThanValue}");

        return guardedValue;
    }

    public static GuardedValue<double> IsLessThanOrEqualTo(this GuardedValue<double> guardedValue, double maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be less than or equal to {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<double> IsBetween(this GuardedValue<double> guardedValue, double minValue, double maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue || guardedValue.ValueToValidate > maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be between {minValue} and {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<double> IsNotNegative(this GuardedValue<double> guardedValue, string? message = null) {
        if (guardedValue.ValueToValidate < 0)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} cannot be negative");

        return guardedValue;
    }
}

