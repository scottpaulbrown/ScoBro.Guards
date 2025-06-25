namespace ScoBro.Guards.UnitTests;

public class CommonValidatorTests {
    [TestCase("TestValue", 10, true)]
    [TestCase("TestValue", 5, false)]
    [TestCase(null, 10, false)]
    [TestCase("", 10, false)]
    [TestCase(" ", 10, false)]
    public void RequiredMaxLengthStringValidator_Should_Validate_Correctly(string? value, int maxLength, bool expected) {
        // Arrange
        var validator = new RequiredMaxLengthStringValidator(maxLength);

        // Act
        var validResult = validator.Validate(value);

        // Assert
        Assert.That(validResult.IsValid, Is.EqualTo(expected));
    }
}