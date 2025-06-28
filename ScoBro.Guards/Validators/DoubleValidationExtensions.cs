namespace ScoBro.Guards;

public static class DoubleValidationExtensions {
    public static ValidationRuleBuilder<T, double> IsNotNegative<T>(
        this ValidationRuleBuilder<T, double> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsPositive<T>(
        this ValidationRuleBuilder<T, double> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsInRange<T>(
        this ValidationRuleBuilder<T, double> builder,
        double minValue,
        double maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minValue && value <= maxValue,
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsEven<T>(
        this ValidationRuleBuilder<T, double> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsOdd<T>(
        this ValidationRuleBuilder<T, double> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, double> builder,
        double minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsLessThan<T>(
        this ValidationRuleBuilder<T, double> builder,
        double maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, double> IsOneOf<T>(
        this ValidationRuleBuilder<T, double> builder,
        params double[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => validValues.Contains(value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");
}

