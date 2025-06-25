using System.Linq.Expressions;

namespace ScoBro.Guards;

public record class Validator {
    public static ValidatorBuilder<TEntity> For<TEntity>() => new ValidatorBuilder<TEntity>();
}

public interface IValidator<T> {
    ValidationResult Validate(T item);
    Task<ValidationResult> ValidateAsync(T item);
}

public record class Validator<T> : Validator, IValidator<T> {
    private readonly List<ValidationRuleSet<T>> _ruleSets = [];

    public Validator() { }

    public Validator(List<ValidationRuleSet<T>> ruleSets) {
        _ruleSets = ruleSets;
    }

    public void AddRuleSet(ValidationRuleSet<T> ruleSet) {
        if (ruleSet == null)
            throw new ArgumentNullException(nameof(ruleSet), "Rule set cannot be null.");

        _ruleSets.Add(ruleSet);
    }

    public ValidationResult Validate(T item) {
        if (_ruleSets.Any(x => x.Rules.Any(a => a.IsAsync)))
            throw new InvalidOperationException("The validator contains async rules. Use ValidateAsync instead.");

        var errors = new List<ValidatorError>();

        foreach (var ruleSet in _ruleSets) {
            foreach (var rule in ruleSet.Rules) {
                var result = rule.ToRule().Validate(item);
                if (!result.IsValid) {
                    errors.AddRange(result.Errors);

                    if (rule.StopValidationIfInvalid)
                        return new ValidationResult(false, errors);
                }
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    protected ValidationRuleBuilder<T, TProp> RuleFor<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        string? fieldName = null) {

        var validationBuilder = new ValidatorBuilder<T>(this);
        return validationBuilder.Rule(propertyExpression, fieldName ?? propertyExpression.GetMemberName());
    }

    public async Task<ValidationResult> ValidateAsync(T item) {
        var errors = new List<ValidatorError>();

        foreach (var ruleSet in _ruleSets) {
            foreach (var rule in ruleSet.Rules) {
                var result = rule.IsAsync ? await rule.ToAsyncRule().ValidateAsync(item) : rule.ToRule().Validate(item);
                if (!result.IsValid) {
                    errors.AddRange(result.Errors);

                    if (rule.StopValidationIfInvalid)
                        return new ValidationResult(false, errors);
                }
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }
}