namespace ScoBro.Guards;

public static class GeneralValidationExtensions {
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
                ValidationConstraint: validationConstraint)
        );

        return builder;
    }

    public static ValidationRuleBuilder<T, TProp> UseValidator<T, TProp>(
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