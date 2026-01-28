namespace ScoBro.Guards;

public static class LongValidationExtensions {
    public static ValidationRuleBuilder<T, long> IsNotNegative<T>(
        this ValidationRuleBuilder<T, long> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsPositive<T>(
        this ValidationRuleBuilder<T, long> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsInRange<T>(
        this ValidationRuleBuilder<T, long> builder,
        long minValue,
        long maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minValue && value <= maxValue,
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsEven<T>(
        this ValidationRuleBuilder<T, long> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsOdd<T>(
        this ValidationRuleBuilder<T, long> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, long> builder,
        long minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsLessThan<T>(
        this ValidationRuleBuilder<T, long> builder,
        long maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long> IsOneOf<T>(
        this ValidationRuleBuilder<T, long> builder,
        params long[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => validValues.Contains(value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");

    // Nullable long extensions
    public static ValidationRuleBuilder<T, long?> IsNotNegative<T>(
        this ValidationRuleBuilder<T, long?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsPositive<T>(
        this ValidationRuleBuilder<T, long?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsInRange<T>(
        this ValidationRuleBuilder<T, long?> builder,
        long minValue,
        long maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || (value.Value >= minValue && value.Value <= maxValue),
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsEven<T>(
        this ValidationRuleBuilder<T, long?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsOdd<T>(
        this ValidationRuleBuilder<T, long?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, long?> builder,
        long minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsLessThan<T>(
        this ValidationRuleBuilder<T, long?> builder,
        long maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, long?> IsOneOf<T>(
        this ValidationRuleBuilder<T, long?> builder,
        params long[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || validValues.Contains(value.Value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");
}
