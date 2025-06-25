using System.Linq.Expressions;

namespace ScoBro.Guards;

public class ValidatorBuilder<T> {
    private readonly Validator<T> _validator;
    private ValidationRuleSet<T>? _currentRuleSet;

    public ValidatorBuilder(Validator<T>? validator = null) {
        _validator = validator ?? new Validator<T>();
    }

    public ValidationRuleBuilder<T, TProp> Rule<TProp>(
        Expression<Func<T, TProp>> propertyExpression,
        string? fieldName = null) {

        BeginNewRuleSet(fieldName);
        return new ValidationRuleBuilder<T, TProp>(this, propertyExpression, fieldName ?? propertyExpression.GetMemberName());
    }

    public void BeginNewRuleSet(string? fieldName = null) {
        _currentRuleSet = new ValidationRuleSet<T>(fieldName: fieldName);
        _validator.AddRuleSet(_currentRuleSet);
    }

    public void AddRule(IValidationRule<T> rule) {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule), "Rule cannot be null.");

        if (_currentRuleSet == null)
            throw new InvalidOperationException("No current rule set. Use Rule() to create a new rule set before adding rules.");

        _currentRuleSet.AddRule(rule);
    }

    public void AddRuleSet(ValidationRuleSet<T> ruleSet) {
        if (ruleSet == null)
            throw new ArgumentNullException(nameof(ruleSet), "Rule set cannot be null.");

        _validator.AddRuleSet(ruleSet);
    }

    public Validator<T> Build() => _validator;
}
