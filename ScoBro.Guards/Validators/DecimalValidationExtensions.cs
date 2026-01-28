namespace ScoBro.Guards;

public static class DecimalValidationExtensions {
    public static ValidationRuleBuilder<T, decimal> IsNotNegative<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsPositive<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsInRange<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        decimal minValue,
        decimal maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minValue && value <= maxValue,
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsEven<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsOdd<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        decimal minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsLessThan<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        decimal maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal> IsOneOf<T>(
        this ValidationRuleBuilder<T, decimal> builder,
        params decimal[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => validValues.Contains(value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");

    // Nullable decimal extensions
    public static ValidationRuleBuilder<T, decimal?> IsNotNegative<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsPositive<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsInRange<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        decimal minValue,
        decimal maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || (value.Value >= minValue && value.Value <= maxValue),
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsEven<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsOdd<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        decimal minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsLessThan<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        decimal maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, decimal?> IsOneOf<T>(
        this ValidationRuleBuilder<T, decimal?> builder,
        params decimal[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || validValues.Contains(value.Value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");
}


