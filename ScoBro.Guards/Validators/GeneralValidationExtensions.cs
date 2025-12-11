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

    public static ValidationRuleBuilder<T, string> DomainEnum<T, TEnum>(
        this ValidationRuleBuilder<T, string> builder,
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

    public static ValidationRuleBuilder<T?, TProp> UseValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        Func<IValidator<TProp>> validatorFactory,
        bool stopIfInvalid = false) {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            var validator = validatorFactory();
            return validator.Validate(value);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> UseValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        IValidator<TProp> validator,
        bool stopIfInvalid = false) {

        if (validator == null)
            throw new ArgumentNullException(nameof(validator), "Validator cannot be null.");

        var validationFunction = new Func<T, ValidationResult>(item => {
            var value = builder.PropertyExpression.Compile()(item);
            return validator.Validate(value);
        });

        builder.AddRule(
            new ValidationRule<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> UseAsyncValidator<T, TProp>(
        this ValidationRuleBuilder<T, TProp> builder,
        Func<IValidator<TProp>> validatorFactory,
        bool stopIfInvalid = false) {

        if (validatorFactory == null)
            throw new ArgumentNullException(nameof(validatorFactory), "Validator factory cannot be null.");

        var validationFunction = new Func<T, Task<ValidationResult>>(async item => {
            var value = builder.PropertyExpression.Compile()(item);
            var validator = validatorFactory();
            return await validator.ValidateAsync(value);
        });

        builder.AddRule(
            new ValidationRuleAsync<T>(validationFunction, StopValidationIfInvalid: stopIfInvalid)
        );

        return builder;
    }
}