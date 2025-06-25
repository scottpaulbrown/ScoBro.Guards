using System.Security.Cryptography.X509Certificates;

namespace ScoBro.Guards.UnitTests;

public class ValidatorTests {
    public class TestEntity {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    [Test]
    public void ValidatorBuilder_CanAddRuleSet() {
        // Arrange
        Validator<TestEntity> validator = Validator.For<TestEntity>()
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
            .Rule(x => x.LastName)
                .IsNotNullOrEmpty()
            .Rule(x => x)
                .Must(x => x.Age > 0, "Age must be greater than zero");

        // Act
        var validationResult = validator.Validate(new TestEntity {
            FirstName = "",
            LastName = "",
            Age = 0
        });

        Assert.That(validationResult.IsValid, Is.False);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(3));
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("FirstName cannot be null or empty."));
        Assert.That(validationResult.Errors[1].ErrorMessage, Is.EqualTo("LastName cannot be null or empty."));
        Assert.That(validationResult.Errors[2].ErrorMessage, Is.EqualTo("Age must be greater than zero"));
    }

    [Test]
    public void ValidatorBuilder_PrimitiveValidation_Works() {
        // Arrange
        Validator<string> validator = Validator.For<string>()
            .Rule(x => x)
                .IsNotNullOrEmpty();

        // Act
        var validationResult = validator.Validate("");

        // Assert
        Assert.That(validationResult.IsValid, Is.False);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(1));
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("Value cannot be null or empty."));
    }

    [Test]
    public async Task ValidatorBuilder_MustAsync_Works() {
        // Arrange
        Validator<TestEntity> validator = Validator.For<TestEntity>()
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
            .Rule(x => x.LastName)
                .IsNotNullOrEmpty()
            .Rule(x => x)
                .MustAsync(async x => {
                    await Task.Delay(1);
                    return x.Age > 0;
                }, "Age must be greater than zero");

        // Act
        var validationResult = await validator.ValidateAsync(new TestEntity {
            FirstName = "John",
            LastName = "Doe",
            Age = -1
        });

        // Assert
        Assert.Multiple(() => {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("Age must be greater than zero"));
        });
    }

    [Test]
    public void ValidatorBuilder_StopIfInvalid_Works() {
        // Arrange
        Validator<TestEntity> validator = Validator.For<TestEntity>()
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty(stopIfInvalid: true)
                .HasMinLength(10);

        // Act
        var validationResult = validator.Validate(new TestEntity {
            FirstName = "",
            LastName = "",
            Age = 0
        });

        // Assert
        Assert.That(validationResult.IsValid, Is.False);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(1));
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("FirstName cannot be null or empty."));
    }
}