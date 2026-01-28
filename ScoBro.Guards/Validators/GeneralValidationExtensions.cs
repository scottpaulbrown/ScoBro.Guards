using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using ScoBro.Foundation;

namespace ScoBro.Guards;

public static class GeneralValidationExtensions {
    public static ValidationRuleBuilder<T, TProp> IsNotNull<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        string errorMessage = "Value cannot be null.",
        bool stopIfInvalid = true,
        bool stopAllIfInvalid = false) =>
        builder.CreateValidationRule(x => x != null, errorMessage, stopIfInvalid, stopAllIfInvalid);

    public static ValidationRuleBuilder<T, Guid> IsNotEmpty<T>(
        this ValidationRuleBuilder<T, Guid> builder,
        string? errorMessage = null,
        bool stopIfInvalid = true,
        bool stopAllIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: x => x != Guid.Empty,
            errorMessage: errorMessage ?? $"{builder.FieldName} cannot be empty",
            stopIfInvalid: stopIfInvalid,
            stopAllIfInvalid: stopAllIfInvalid);

    public static ValidationRuleBuilder<T, Guid?> IsNotEmpty<T>(
        this ValidationRuleBuilder<T, Guid?> builder,
        string? errorMessage = null,
        bool stopIfInvalid = true,
        bool stopAllIfInvalid = false) =>
        builder.CreateValidationRule(
            validateValue: x => x == null || x.Value != Guid.Empty,
            errorMessage: errorMessage ?? $"{builder.FieldName} cannot be empty",
            stopIfInvalid: stopIfInvalid,
            stopAllIfInvalid: stopAllIfInvalid);

    public static ValidationRuleBuilder<T, string?> DomainEnum<T, TEnum>(
        this ValidationRuleBuilder<T, string?> builder,
        string? errorMessage = null,
        bool stopIfInvalid = true,
        bool stopAllIfInvalid = false)
        where TEnum : EnumBase<TEnum> =>
        builder.CreateValidationRule(
            validateValue: x => !string.IsNullOrEmpty(x) && EnumBase<TEnum>.TryParse(x, out var _),
            errorMessage: errorMessage ?? $"Invalid value for domain enum {typeof(TEnum).Name}",
            stopIfInvalid: stopIfInvalid,
            stopAllIfInvalid: stopAllIfInvalid);

    public static ValidationRuleBuilder<T, TProp> Optional<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder) {
        builder.MakeOptional();
        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> Must<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        Func<TProp, bool> predicate,
        string errorMessage) {

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);

            if (!predicate(value))
                return ValidationResult.Invalid(errorMessage);

            return ValidationResult.Valid();
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> MustAsync<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        Func<TProp, Task<bool>> predicate,
        string errorMessage) {

        var validationFunction = new Func<T, Task<ValidationResult>>(async item => {
            var value = builder.PropertyExpression.Compile()(item);
            var result = await predicate(value);

            if (!result)
                return ValidationResult.Invalid(errorMessage);

            return ValidationResult.Valid();
        });

        builder.AddRule(
            new ValidationRuleAsync<T>(validationFunction)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> CreateValidationRule<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        Func<TProp, bool> validateValue,
        string errorMessage,
        bool stopIfInvalid = false,
        bool stopAllIfInvalid = false,
        ValidationConstraint? validationConstraint = null) {

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            if (!validateValue(value))
                return ValidationResult.Invalid(errorMessage);

            return ValidationResult.Valid();
        });

        builder.AddRule(
            new ValidationRule<T>(
                ValidateFunction: validationFunction,
                StopValidationIfInvalid: stopIfInvalid,
                StopAllIfInvalid: stopAllIfInvalid,
                ValidationConstraint: validationConstraint)
        );

        return builder;
    }

    // Generic overloads - these work with any type including nullable reference types
    // The trick is to use object-based validation internally
    public static ValidationRuleBuilder<T, TPropBuilder> UseValidator<T, TPropBuilder, TPropValidator>(
        this ValidationRuleBuilder<T, TPropBuilder> builder,
        IValidator<TPropValidator> validator,
        bool stopIfInvalid = false) {

        if (validator == null)
            throw new ArgumentNullException(nameof(validator), "Validator cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if value is null
            if (value == null)
                return ValidationResult.Valid();
            // Cast the value to the validator's expected type
            return validator.Validate((TPropValidator)(object)value!);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TPropBuilder> UseValidator<T, TPropBuilder, TPropValidator>(
        this ValidationRuleBuilder<T, TPropBuilder> builder,
        Func<IValidator<TPropValidator>> validatorFactory,
        bool stopIfInvalid = false) {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if value is null
            if (value == null)
                return ValidationResult.Valid();
            var validator = validatorFactory();
            // Cast the value to the validator's expected type
            return validator.Validate((TPropValidator)(object)value!);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    // Nullable overloads for UseValidator to support using non-nullable validators with nullable properties
    // Nullable value type overloads - these allow using non-nullable validators with nullable value type properties
    public static ValidationRuleBuilder<T, TProp?> UseValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp?> builder,
        IValidator<TProp> validator,
        bool stopIfInvalid = false) where TProp : struct {

        if (validator == null)
            throw new ArgumentNullException(nameof(validator), "Validator cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if null
            if (value == null)
                return ValidationResult.Valid();
            return validator.Validate(value.Value);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp?> UseValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp?> builder,
        Func<IValidator<TProp>> validatorFactory,
        bool stopIfInvalid = false) where TProp : struct {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if null
            if (value == null)
                return ValidationResult.Valid();
            var validator = validatorFactory();
            return validator.Validate(value.Value);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }


    // Generic overload with separate type parameters for builder and validator types
    public static ValidationRuleBuilder<T, TPropBuilder> UseAsyncValidator<T, TPropBuilder, TPropValidator>(
        this ValidationRuleBuilder<T, TPropBuilder> builder,
        Func<IValidator<TPropValidator>> validatorFactory,
        bool stopIfInvalid = false) {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, Task<ValidationResult>>(async item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if value is null
            if (value == null)
                return ValidationResult.Valid();
            var validator = validatorFactory();
            // Cast the value to the validator's expected type
            return await validator.ValidateAsync((TPropValidator)(object)value!);
        });

        builder.AddRule(
            new ValidationRuleAsync<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    // Nullable overloads for UseAsyncValidator
    public static ValidationRuleBuilder<T, TProp?> UseAsyncValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp?> builder,
        Func<IValidator<TProp>> validatorFactory,
        bool stopIfInvalid = false) where TProp : struct {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, Task<ValidationResult>>(async item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if null
            if (value == null)
                return ValidationResult.Valid();
            var validator = validatorFactory();
            return await validator.ValidateAsync(value.Value);
        });

        builder.AddRule(
            new ValidationRuleAsync<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, string?> UseAsyncValidator<T>(
        this ValidationRuleBuilder<T, string?> builder,
        Func<IValidator<string>> validatorFactory,
        bool stopIfInvalid = false) {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, Task<ValidationResult>>(async item => {
            var value = builder.PropertyExpression.Compile()(item);
            // Skip validation if null
            if (value == null)
                return ValidationResult.Valid();
            var validator = validatorFactory();
            return await validator.ValidateAsync(value);
        });

        builder.AddRule(
            new ValidationRuleAsync<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }
}