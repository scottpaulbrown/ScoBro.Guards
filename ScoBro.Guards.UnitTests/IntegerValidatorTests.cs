namespace ScoBro.Guards.UnitTests;

public class IntegerValidatorTest {
    private class TestObject {
        public int Value { get; set; }
    }

    [TestCase(0)]
    [TestCase(5)]
    public void IsNotNegative_Valid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsNotNegative();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void IsNotNegative_Invalid() {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsNotNegative();
        var result = validator.Validate(new TestObject { Value = -1 });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("Value cannot be negative."));
    }

    [TestCase(1)]
    [TestCase(100)]
    public void IsPositive_Valid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsPositive();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(0)]
    [TestCase(-5)]
    public void IsPositive_Invalid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsPositive();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("Value must be positive."));
    }

    [TestCase(5, 1, 10)]
    [TestCase(1, 1, 1)]
    [TestCase(10, 10, 20)]
    public void IsInRange_Valid(int value, int min, int max) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsInRange(min, max);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(0, 1, 10)]
    [TestCase(11, 1, 10)]
    public void IsInRange_Invalid(int value, int min, int max) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsInRange(min, max);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo($"Value must be between {min} and {max}."));
    }

    [TestCase(2)]
    [TestCase(0)]
    [TestCase(-4)]
    public void IsEven_Valid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsEven();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(1)]
    [TestCase(-3)]
    public void IsEven_Invalid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsEven();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("Value must be even."));
    }

    [TestCase(1)]
    [TestCase(-3)]
    public void IsOdd_Valid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsOdd();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(2)]
    [TestCase(-4)]
    public void IsOdd_Invalid(int value) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsOdd();
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo("Value must be odd."));
    }

    [TestCase(6, 5)]
    [TestCase(10, 0)]
    public void IsGreaterThan_Valid(int value, int min) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsGreaterThan(min);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(5, 5)]
    [TestCase(4, 5)]
    public void IsGreaterThan_Invalid(int value, int min) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsGreaterThan(min);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo($"Value must be greater than {min}."));
    }

    [TestCase(4, 5)]
    [TestCase(-1, 0)]
    public void IsLessThan_Valid(int value, int max) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsLessThan(max);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(5, 5)]
    [TestCase(6, 5)]
    public void IsLessThan_Invalid(int value, int max) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsLessThan(max);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Is.EqualTo($"Value must be less than {max}."));
    }

    [TestCase(1, new int[] { 1, 2, 3 })]
    [TestCase(3, new int[] { 1, 2, 3 })]
    public void IsOneOf_Valid(int value, int[] validValues) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsOneOf(validValues);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase(4, new int[] { 1, 2, 3 })]
    [TestCase(0, new int[] { 1, 2, 3 })]
    public void IsOneOf_Invalid(int value, int[] validValues) {
        Validator<TestObject> validator = Validator.For<TestObject>()
            .Rule(x => x.Value).IsOneOf(validValues);
        var result = validator.Validate(new TestObject { Value = value });
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.First().ErrorMessage, Does.Contain("must be one of"));
    }
}