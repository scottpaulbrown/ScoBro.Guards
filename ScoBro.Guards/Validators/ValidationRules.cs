namespace ScoBro.Guards;

public record class ValidationRuleSet<T> {
    private readonly List<IValidationRule<T>> _rules;

    public ValidationRuleSet(string memberName, List<IValidationRule<T>>? rules = null, string? fieldName = null) {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentException("Member name cannot be null or whitespace.", nameof(memberName));

        MemberName = memberName;
        FieldName = fieldName ?? memberName;
        _rules = rules ?? [];
    }

    public string MemberName { get; }
    public string? FieldName { get; }
    public bool IsOptional { get; private set; }

    public IReadOnlyList<IValidationRule<T>> Rules => _rules;

    public void AddRule(IValidationRule<T> rule) {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule), "Rule cannot be null.");
        _rules.Add(rule);
    }

    public void MakeOptional() => IsOptional = true;

    public bool HasFieldName() => !string.IsNullOrWhiteSpace(FieldName);
}

public interface IValidationRule<T> {
    bool IsAsync { get; }
    bool HasErrorMessage();
    bool StopValidationIfInvalid { get; }
    bool StopAllIfInvalid { get; }

    ValidationConstraint? ValidationConstraint { get; }

    string? ErrorMessage { get; }

    ValidationRule<T> ToRule();
    ValidationRuleAsync<T> ToAsyncRule();
}

public record class ValidationRuleBase<TEntity>(
    bool IsAsync = false,
    bool StopValidationIfInvalid = false,
    bool StopAllIfInvalid = false,
    ValidationConstraint? ValidationConstraint = null) {

    public string? ErrorMessage { get; private set; }

    public void SetErrorMessage(string errorMessage) {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message cannot be null or whitespace.", nameof(errorMessage));

        ErrorMessage = errorMessage;
    }

    public bool HasErrorMessage() => !string.IsNullOrWhiteSpace(ErrorMessage);

    public ValidationRule<TEntity> ToRule() {
        if (IsAsync)
            throw new InvalidOperationException("Cannot convert to sync rule when IsAsync is true.");

        return (ValidationRule<TEntity>)this;
    }

    public ValidationRuleAsync<TEntity> ToAsyncRule() {
        if (!IsAsync)
            throw new InvalidOperationException("Cannot convert to async rule when IsAsync is false.");

        return (ValidationRuleAsync<TEntity>)this;
    }
}

public record class ValidationRule<TEntity>(
    Func<TEntity, ValidationResult> ValidateFunction,
    bool StopValidationIfInvalid = false,
    bool StopAllIfInvalid = false,
    ValidationConstraint? ValidationConstraint = null)
    : ValidationRuleBase<TEntity>(
        StopValidationIfInvalid: StopValidationIfInvalid,
        StopAllIfInvalid: StopAllIfInvalid,
        ValidationConstraint: ValidationConstraint
    ), IValidationRule<TEntity> {

    public ValidationResult Validate(TEntity value) {
        return ValidateFunction(value);
    }
}

public record class ValidationRuleAsync<TEntity>(
    Func<TEntity, Task<ValidationResult>> ValidateFunction,
    bool StopValidationIfInvalid = false,
    bool StopAllIfInvalid = false,
    ValidationConstraint? ValidationConstraint = null)
: ValidationRuleBase<TEntity>(
    IsAsync: true,
    StopValidationIfInvalid: StopValidationIfInvalid,
    StopAllIfInvalid: StopAllIfInvalid,
    ValidationConstraint: ValidationConstraint
), IValidationRule<TEntity> {

    public async Task<ValidationResult> ValidateAsync(TEntity value) {
        return await ValidateFunction(value);
    }
}

