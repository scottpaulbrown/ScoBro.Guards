using System.Data;

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
        var validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
            .Rule(x => x.LastName)
                .IsNotNullOrEmpty()
            .Rule(x => x)
                .Must(x => x.Age > 0, "Age must be greater than zero")
        );

        // Act
        var validationResult = validator.Validate(new TestEntity {
            FirstName = "",
            LastName = "",
            Age = 0
        });

        Assert.Multiple(() => {
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors, Has.Count.EqualTo(3));
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("FirstName cannot be null or empty."));
            Assert.That(validationResult.Errors[1].ErrorMessage, Is.EqualTo("LastName cannot be null or empty."));
            Assert.That(validationResult.Errors[2].ErrorMessage, Is.EqualTo("Age must be greater than zero"));
        });
    }

    [Test]
    public void ValidatorBuilder_PrimitiveValidation_Works() {
        // Arrange
        Validator<string> validator = Validator.For<string>(builder => builder
            .Rule(x => x)
                .IsNotNullOrEmpty()
        );

        // Act
        var validationResult = validator.Validate("");

        Assert.Multiple(() => {
            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("Value cannot be null or empty."));
        });
    }

    [Test]
    public async Task ValidatorBuilder_MustAsync_Works() {
        // Arrange
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
            .Rule(x => x.LastName)
                .IsNotNullOrEmpty()
            .Rule(x => x)
                .MustAsync(async x => {
                    await Task.Delay(1);
                    return x.Age > 0;
                }, "Age must be greater than zero")
        );

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
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
                .HasMinLength(10)
        );

        // Act
        var validationResult = validator.Validate(new TestEntity {
            FirstName = "",
            LastName = "",
            Age = 0
        });

        Assert.Multiple(() => {
            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("FirstName cannot be null or empty."));
        });
    }

    [Test]
    public void ValidatorBuilder_ProducesConstraints() {
        // Arrange
        var validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.FirstName)
                .IsNotNullOrEmpty()
                .HasMaxLength(20)
            .Rule(x => x.LastName)
                .IsNotNullOrEmpty()
                .HasMaxLength(30)
            .Rule(x => x.Age)
                .IsPositive()
        );

        // Act
        var constraints = validator.GetValidationConstraints();

        Assert.Multiple(() => {
            Assert.That(constraints, Has.Count.EqualTo(2));
            Assert.That(constraints[0].Type, Is.EqualTo(ValidationConstraintType.MaxLength));
            Assert.That(constraints[0].ToStringMaxLengthConstraint().MaxLength, Is.EqualTo(20));
            Assert.That(constraints[0].ToStringMaxLengthConstraint().MemberName, Is.EqualTo("FirstName"));
        });
    }
}