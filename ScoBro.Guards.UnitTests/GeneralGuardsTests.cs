using System.Security.Cryptography.X509Certificates;

namespace ScoBro.Guards.UnitTests;

public class GeneralGuardsTests {
    [Test]
    public void IsNotNull_NullValue_Throws() {
        object test = null;

        Assert.Throws<ArgumentNullException>(() => Guard.For(test).IsNotNull());
    }

    [Test]
    public void IsNotNull_ValidValue_Successful() {
        var test = new GeneralGuardTestClass();
        GeneralGuardTestClass value = Guard.For(test).IsNotNull();
        Assert.That(value, Is.EqualTo(test));
    }

    [Test]
    public void IsNotTrue_TrueValue_Throws() {
        GeneralGuardTestClass testClass = new() { Name = "test" };

        Assert.Throws<ArgumentException>(() =>
            Guard.For(testClass).IsNotTrue(value => value.Name.Equals("test")));
    }

    [Test]
    public void IsNotTrue_FalseValue_Successful() {
        GeneralGuardTestClass testClass = new() { Name = "test" };

        GeneralGuardTestClass value =
            Guard.For(testClass).IsNotTrue(value => value.Equals("non-test"));

        Assert.That(value, Is.EqualTo(testClass));
    }

    [TestCase("test", true)]
    [TestCase("", false)]
    public async Task UseAsyncValidator_Tests(string testValue, bool expectedResult) {
        Validator<string> testValidator = Validator.For<string>().Rule(x => x)
           .MustAsync(val => Task.FromResult(!string.IsNullOrWhiteSpace(val)), "Value cannot be empty");

        Validator<string> testDependendValidator = Validator.For<string>()
            .Rule(x => x)
            .UseAsyncValidator(() => testValidator);

        var result = await testDependendValidator.ValidateAsync(testValue);
        Assert.That(result.IsValid, Is.EqualTo(expectedResult));
    }

    [TestCase("test", true)]
    [TestCase("", false)]
    public void UseValidator_Tests(string testValue, bool expectedResult) {
        Validator<string> testValidator = Validator.For<string>().Rule(x => x)
            .IsNotNullOrEmpty();

        Validator<string> testDependendValidator = Validator.For<string>()
            .Rule(x => x)
            .UseValidator(() => testValidator);

        var result = testDependendValidator.Validate(testValue);
        Assert.That(result.IsValid, Is.EqualTo(expectedResult));
    }
}

public class GeneralGuardTestClass {
    public string Name { get; set; }
}
