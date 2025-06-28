using System.Linq.Expressions;

namespace ScoBro.Guards;

public class ValidationRuleBuilder<TEntity, TProperty> {
    private readonly List<IValidationRule<TEntity>> _rules = [];

    public ValidationRuleBuilder(
        ValidatorBuilder<TEntity> validationBuilder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        string memberName,
        string? fieldName = null) {

        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentException("Member name cannot be null or whitespace.", nameof(memberName));

        ValidatorBuilder = validationBuilder ??
            throw new ArgumentNullException(nameof(validationBuilder), "Validation builder cannot be null.");
        PropertyExpression = propertyExpression ??
            throw new ArgumentNullException(nameof(propertyExpression), "Property expression cannot be null.");
        MemberName = memberName;
        FieldName = fieldName ?? memberName;
    }

    public string MemberName { get; }
    public string? FieldName { get; }
    public ValidatorBuilder<TEntity> ValidatorBuilder { get; }
    public Expression<Func<TEntity, TProperty>> PropertyExpression { get; }

    public void AddRule(IValidationRule<TEntity> rule) {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule), "Rule cannot be null.");

        ValidatorBuilder.AddRule(rule);
    }

    public ValidationRuleBuilder<TEntity, TProp> Rule<TProp>(
        Expression<Func<TEntity, TProp>> propertyExpression,
        string? fieldName = null
    ) => ValidatorBuilder.Rule(propertyExpression, fieldName);

    private Validator<TEntity> Build() => ValidatorBuilder.Build();

    public static implicit operator Validator<TEntity>(ValidationRuleBuilder<TEntity, TProperty> builder) =>
        builder.Build();
}