namespace ScoBro.Guards;

public static class IntegerValidationExtensions {
    public static ValidationRuleBuilder<T, int> IsNotNegative<T>(
        this ValidationRuleBuilder<T, int> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsPositive<T>(
        this ValidationRuleBuilder<T, int> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsInRange<T>(
        this ValidationRuleBuilder<T, int> builder,
        int minValue,
        int maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minValue && value <= maxValue,
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsEven<T>(
        this ValidationRuleBuilder<T, int> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsOdd<T>(
        this ValidationRuleBuilder<T, int> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, int> builder,
        int minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsLessThan<T>(
        this ValidationRuleBuilder<T, int> builder,
        int maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, int> IsOneOf<T>(
        this ValidationRuleBuilder<T, int> builder,
        params int[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => validValues.Contains(value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");
}