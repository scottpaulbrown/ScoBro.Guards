using System.Linq.Expressions;
using ScoBro.Foundation;

namespace ScoBro.Guards;

public record class Validator {
    public static ValidatorBuilder<TEntity> For<TEntity>() => new();

    public static Validator<TEntity> For<TEntity>(Action<ValidatorBuilder<TEntity>> configureAction) {
        var validator = new Validator<TEntity>();
        var builder = new ValidatorBuilder<TEntity>(validator);
        configureAction(builder);
        return builder.Build();
    }

    public static Validator<TEntity> Single<TEntity>(
        string fieldName,
        Action<ValidationRuleBuilder<TEntity, TEntity>> configureAction) {

        var validator = new Validator<TEntity>();
        var builder = new ValidatorBuilder<TEntity>(validator);
        configureAction(builder.Rule(x => x, fieldName));
        return builder.Build();
    }

    public static ValidationResult Validate<TItem>(TItem item, Action<ValidatorBuilder<TItem>> configureAction) {
        var validator = For(configureAction);
        return validator.Validate(item);
    }
}

public interface IValidator {
    List<ValidationConstraint> GetValidationConstraints();
}

public interface IValidator<T> : IValidator {
    IReadOnlyList<ValidationRuleSet<T>> RuleSets { get; }

    ValidationResult Validate(T item);
    Task<ValidationResult> ValidateAsync(T item);
    void AddRuleSets(IEnumerable<ValidationRuleSet<T>> ruleSets);
}

public record class Validator<T> : Validator, IValidator<T> {
    private readonly List<ValidationRuleSet<T>> _ruleSets = [];

    public IReadOnlyList<ValidationRuleSet<T>> RuleSets => _ruleSets;

    public Validator() { }

    public Validator(List<ValidationRuleSet<T>> ruleSets) {
        _ruleSets = ruleSets;
    }

    public Validator(Action<ValidatorBuilder<T>> configureAction) {
        var builder = new ValidatorBuilder<T>(this);
        configureAction(builder);
    }

    public void AddRuleSet(ValidationRuleSet<T> ruleSet) {
        if (ruleSet == null)
            throw new ArgumentNullException(nameof(ruleSet), "Rule set cannot be null.");

        _ruleSets.Add(ruleSet);
    }

    public void AddRuleSets(IEnumerable<ValidationRuleSet<T>> ruleSets) {
        if (ruleSets == null)
            throw new ArgumentNullException(nameof(ruleSets), "Rule sets cannot be null.");

        foreach (var ruleSet in ruleSets) {
            AddRuleSet(ruleSet);
        }
    }

    public ValidationResult Validate(T item) {
        if (_ruleSets.Any(x => x.Rules.Any(a => a.IsAsync)))
            throw new InvalidOperationException("The validator contains async rules. Use ValidateAsync instead.");

        var errors = new List<ValidatorError>();

        foreach (var ruleSet in _ruleSets) {
            bool stopAllOccurred = false;

            foreach (var rule in ruleSet.Rules) {
                var result = rule.ToRule().Validate(item);
                if (!result.IsValid) {
                    errors.AddRange(result.Errors);

                    if (rule.StopAllIfInvalid) {
                        stopAllOccurred = true;
                        break;
                    }

                    if (rule.StopValidationIfInvalid)
                        break;
                }
            }

            if (stopAllOccurred) break;
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
            bool stopAllOccurred = false;

            foreach (var rule in ruleSet.Rules) {
                var result = rule.IsAsync ? await rule.ToAsyncRule().ValidateAsync(item) : rule.ToRule().Validate(item);
                if (!result.IsValid) {
                    errors.AddRange(result.Errors);

                    if (rule.StopAllIfInvalid) {
                        stopAllOccurred = true;
                        break;
                    }

                    if (rule.StopValidationIfInvalid)
                        break;
                }
            }

            if (stopAllOccurred) break;
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    public List<ValidationConstraint> GetValidationConstraints() {
        return _ruleSets
            .SelectMany(rs => rs.Rules)
            .Where(r => r.ValidationConstraint != null)
            .Select(r => r.ValidationConstraint!)
            .Distinct()
            .ToList();
    }

    public SimpleResult<T> ValidateToSimpleResult(T item) {
        var validationResult = Validate(item);
        if (validationResult.IsValid) {
            return SimpleResult.Ok(item);
        }

        return SimpleResult.FailWIthValidationErrors<T>(
            validationResult.Errors.Select(e => e.ErrorMessage).ToList());
    }
}