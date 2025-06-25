namespace ScoBro.Guards;

public enum ValidationConstraintType {
    MaxLength
}

public record class ValidationConstraint(ValidationConstraintType Type, string MemberName);

public record class StringMaxLengthConstraint(string MemberName, int MaxLength) :
    ValidationConstraint(ValidationConstraintType.MaxLength, MemberName);

