using System.Linq.Expressions;

namespace ScoBro.Guards;

public class ValidationRuleBuilder<TEntity, TProperty> {
    private readonly List<IValidationRule<TEntity>> _rules = [];

    public ValidationRuleBuilder(
        ValidatorBuilder<TEntity> validationBuilder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        string? fieldName = null) {

        ValidatorBuilder = validationBuilder;
        PropertyExpression = propertyExpression;
        MemberName = propertyExpression.GetMemberName();
        FieldName = fieldName ?? MemberName;
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
    ) {

        ValidatorBuilder.BeginNewRuleSet(fieldName);

        return new ValidationRuleBuilder<TEntity, TProp>(
            validationBuilder: ValidatorBuilder,
            propertyExpression: propertyExpression,
            fieldName: fieldName ?? propertyExpression.GetMemberName()
        );
    }

    private Validator<TEntity> Build() => ValidatorBuilder.Build();

    public static implicit operator Validator<TEntity>(ValidationRuleBuilder<TEntity, TProperty> builder) =>
        builder.Build();
}