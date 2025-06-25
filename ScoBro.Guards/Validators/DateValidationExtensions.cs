namespace ScoBro.Guards;

public static class DateValidationExtensions {
    public static ValidationRuleBuilder<T, DateTime> IsNotDefault<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value != default,
            errorMessage: $"{builder.FieldName} cannot be the default date.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime> IsNotInFuture<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value <= DateTime.UtcNow,
            errorMessage: $"{builder.FieldName} cannot be in the future.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime> IsNotInPast<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= DateTime.UtcNow,
            errorMessage: $"{builder.FieldName} cannot be in the past.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime> IsAfter<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        DateTime minDate,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value > minDate,
            errorMessage: $"{builder.FieldName} must be after {minDate:O}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime> IsBefore<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        DateTime maxDate,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value < maxDate,
            errorMessage: $"{builder.FieldName} must be before {maxDate:O}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime> IsBetween<T>(
        this ValidationRuleBuilder<T, DateTime> builder,
        DateTime minDate,
        DateTime maxDate,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value >= minDate && value <= maxDate,
            errorMessage: $"{builder.FieldName} must be between {minDate:O} and {maxDate:O}.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, DateTime?> IsNotNullOrDefault<T>(
        this ValidationRuleBuilder<T, DateTime?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value.HasValue && value.Value != default,
            errorMessage: $"{builder.FieldName} cannot be null or default.",
            stopIfInvalid: stopIfInvalid);
}