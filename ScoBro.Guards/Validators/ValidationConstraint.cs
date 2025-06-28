namespace ScoBro.Guards;

public enum ValidationConstraintType {
    MaxLength
}

public record class ValidationConstraint(ValidationConstraintType Type, string MemberName) {
    public StringMaxLengthConstraint ToStringMaxLengthConstraint() {
        if (Type != ValidationConstraintType.MaxLength)
            throw new InvalidOperationException($"Cannot convert {Type} to StringMaxLengthConstraint.");

        return (StringMaxLengthConstraint)this;
    }
}

public record class StringMaxLengthConstraint(string MemberName, int MaxLength) :
    ValidationConstraint(ValidationConstraintType.MaxLength, MemberName);

