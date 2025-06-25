namespace ScoBro.Guards;

public static class ShortValidationExtensions {
    public static ValidationRuleBuilder<T, short> IsNotNegative<T>(
        this ValidationRuleBuilder<T, short> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= 0,
            errorMessage: $"{builder.FieldName} cannot be negative.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsPositive<T>(
        this ValidationRuleBuilder<T, short> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > 0,
            errorMessage: $"{builder.FieldName} must be positive.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsInRange<T>(
        this ValidationRuleBuilder<T, short> builder,
        short minValue,
        short maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minValue && value <= maxValue,
            errorMessage: $"{builder.FieldName} must be between {minValue} and {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsEven<T>(
        this ValidationRuleBuilder<T, short> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 == 0,
            errorMessage: $"{builder.FieldName} must be even.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsOdd<T>(
        this ValidationRuleBuilder<T, short> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value % 2 != 0,
            errorMessage: $"{builder.FieldName} must be odd.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsGreaterThan<T>(
        this ValidationRuleBuilder<T, short> builder,
        short minValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minValue,
            errorMessage: $"{builder.FieldName} must be greater than {minValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsLessThan<T>(
        this ValidationRuleBuilder<T, short> builder,
        short maxValue,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxValue,
            errorMessage: $"{builder.FieldName} must be less than {maxValue}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, short> IsOneOf<T>(
        this ValidationRuleBuilder<T, short> builder,
        params short[] validValues) =>
        builder.CreateValidationRule(
            validateValue: value => validValues.Contains(value),
            errorMessage: $"{builder.FieldName} must be one of: {string.Join(", ", validValues)}.");
}
