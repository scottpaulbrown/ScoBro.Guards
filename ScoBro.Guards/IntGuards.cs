namespace ScoBro.Guards;

public static class IntGuards {
    public static GuardedValue<int> IsGreaterThan(this GuardedValue<int> guardedValue, int greaterThanValue, string? message = null) {
        if (guardedValue.ValueToValidate <= greaterThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be greater than {greaterThanValue}");
        
        return guardedValue;
    }

    public static GuardedValue<int> IsGreaterThanOrEqualTo(this GuardedValue<int> guardedValue, int minValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be greater than or equal to {minValue}");

        return guardedValue;
    }

    public static GuardedValue<int> IsLessThan(this GuardedValue<int> guardedValue, int lessThanValue, string? message = null) {
        if (guardedValue.ValueToValidate >= lessThanValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be less than {lessThanValue}");

        return guardedValue;
    }

    public static GuardedValue<int> IsLessThanOrEqualTo(this GuardedValue<int> guardedValue, int maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName, message ?? $"Value must be less than or equal to {maxValue}");

        return guardedValue;
    }

    public static GuardedValue<int> IsBetween(this GuardedValue<int> guardedValue, int minValue, int maxValue, string? message = null) {
        if (guardedValue.ValueToValidate < minValue || guardedValue.ValueToValidate > maxValue)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} must be between {minValue} and {maxValue}");
        
        return guardedValue;
    }

    public static GuardedValue<int> IsNotNegative(this GuardedValue<int> guardedValue, string? message = null) {
        if (guardedValue.ValueToValidate < 0)
            throw new ArgumentOutOfRangeException(guardedValue.FieldName,
                message ?? $"{guardedValue.FieldName} cannot be negative");

        return guardedValue;
    }   
}
