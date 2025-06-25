
namespace ScoBro.Guards;
public static class DecimalGuards {
    public static GuardedValue<decimal> IsGreaterThan(this GuardedValue<decimal> guardedValue, decimal greaterThanValue, string? message = null) {
        if (guardedValue.ValueToValidate <= greaterThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be greater than {greaterThanValue}");

        return guardedValue;
    }

    public static GuardedValue<decimal> IsGreaterThanOrEqualTo(this GuardedValue<decimal> guardedValue, decimal minValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be greater than or equal to {minValue}");

        return guardedValue;
    }

    public static GuardedValue<decimal> IsLessThan(this GuardedValue<decimal> guardedValue, decimal lessThanValue, string? message = null) {
        if (guardedValue.ValueToValidate >= lessThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be less than {lessThanValue}");

        return guardedValue;
    }

    public static GuardedValue<decimal> IsLessThanOrEqualTo(this GuardedValue<decimal> guardedValue, decimal maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be less than or equal to {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<decimal> IsBetween(this GuardedValue<decimal> guardedValue, decimal minValue, decimal maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue || guardedValue.ValueToValidate > maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be between {minValue} and {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<decimal> IsNotNegative(this GuardedValue<decimal> guardedValue, string? message = null) {
        if (guardedValue.ValueToValidate < 0)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} cannot be negative");

        return guardedValue;
    }
}

