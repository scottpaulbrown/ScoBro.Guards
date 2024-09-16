using System.Text.RegularExpressions;

namespace ScoBro.Guards;
public static class StringGuards {
    public static GuardedValue<string> IsNotNullOrEmpty(this GuardedValue<string> guardedValue, string? message = null) {
        if (string.IsNullOrEmpty(guardedValue.ValueToValidate)) 
                throw new ArgumentNullException(guardedValue.FieldName, message);
        
        return guardedValue;
    }

    public static GuardedValue<string> HasMaxLength(this GuardedValue<string> guardedValue, int maxLength, string? message = null) {
        if (string.IsNullOrEmpty(guardedValue.ValueToValidate) || guardedValue.ValueToValidate.Length > maxLength)
            throw new ArgumentOutOfRangeException(
                guardedValue.FieldName, message ?? $"{guardedValue.FieldName} cannot exceed {maxLength} characters");

        return guardedValue;
    }

    public static GuardedValue<string> MatchesRegEx(this GuardedValue<string> guardedValue, string regEx, string? message = null) {
        if (guardedValue.ValueToValidate != null && !Regex.IsMatch(guardedValue.ValueToValidate, regEx, RegexOptions.IgnoreCase))
            throw new ArgumentException($"{guardedValue.FieldName} is not in correct format");

        return guardedValue;
    }
}
