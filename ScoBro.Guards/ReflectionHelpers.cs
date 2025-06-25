using System.Linq.Expressions;

namespace ScoBro.Guards;

public static class ExpressionHelper {
    public static string GetMemberName<TEntity, TProp>(this Expression<Func<TEntity, TProp>> expression, string defaultName = "Value") {
        if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression memberExpr) {
            return memberExpr.Member.Name;
        }

        if (expression.Body is MemberExpression member) {
            return member.Member.Name;
        }

        return defaultName;
    }
}