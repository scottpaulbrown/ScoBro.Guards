namespace ScoBro.Guards;

public static class StringValidationExtensions {
    public static ValidationRuleBuilder<T, string?> IsNotNullOrEmpty<T>(
        this ValidationRuleBuilder<T, string?> builder,
        bool stopIfInvalid = true) =>
        builder.CreateValidationRule(
            validateValue: value => !string.IsNullOrEmpty(value?.Trim()),
            errorMessage: $"{builder.FieldName} cannot be null or empty.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> HasMaxLength<T>(
        this ValidationRuleBuilder<T, string?> builder,
        int maxLength,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: v => v == null || v.Length <= maxLength,
            errorMessage: $"{builder.FieldName} cannot exceed {maxLength} characters.",
            stopIfInvalid: stopIfInvalid,
            validationConstraint: new StringMaxLengthConstraint(builder.MemberName, maxLength));

    public static ValidationRuleBuilder<T, string?> HasMinLength<T>(
        this ValidationRuleBuilder<T, string?> builder,
        int minLength,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: v => v == null || v.Length >= minLength,
            errorMessage: $"{builder.FieldName} must be at least {minLength} characters.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> IsNotNullOrWhiteSpace<T>(
        this ValidationRuleBuilder<T, string?> builder,
        bool stopIfInvalid = true) =>
        builder.CreateValidationRule(
            validateValue: value => !string.IsNullOrWhiteSpace(value),
            errorMessage: $"{builder.FieldName} cannot be null or whitespace.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> MatchesRegex<T>(
        this ValidationRuleBuilder<T, string?> builder,
        string pattern,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || System.Text.RegularExpressions.Regex.IsMatch(value, pattern),
            errorMessage: $"{builder.FieldName} is not in the correct format.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> DoesNotContain<T>(
        this ValidationRuleBuilder<T, string?> builder,
        string substring,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || !value.Contains(substring),
            errorMessage: $"{builder.FieldName} cannot contain '{substring}'.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> Contains<T>(
        this ValidationRuleBuilder<T, string?> builder,
        string substring,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.Contains(substring),
            errorMessage: $"{builder.FieldName} must contain '{substring}'.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> StartsWith<T>(
        this ValidationRuleBuilder<T, string?> builder,
        string prefix,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.StartsWith(prefix),
            errorMessage: $"{builder.FieldName} must start with '{prefix}'.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> EndsWith<T>(
        this ValidationRuleBuilder<T, string?> builder,
        string suffix,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || value.EndsWith(suffix),
            errorMessage: $"{builder.FieldName} must end with '{suffix}'.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> IsEmail<T>(
        this ValidationRuleBuilder<T, string?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
            errorMessage: $"{builder.FieldName} must be a valid email address.",
            stopIfInvalid: stopIfInvalid);

    public static ValidationRuleBuilder<T, string?> IsGuid<T>(
        this ValidationRuleBuilder<T, string?> builder,
        bool stopIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: value => value == null || Guid.TryParse(value, out _),
            errorMessage: $"{builder.FieldName} must be a valid GUID.",
            stopIfInvalid: stopIfInvalid);
}