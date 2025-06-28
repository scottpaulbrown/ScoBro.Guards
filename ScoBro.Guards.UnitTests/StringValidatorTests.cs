namespace ScoBro.Guards.UnitTests;

public class StringValidatorTests {
    public class TestEntity {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Test]
    public void StringValidator_IsNotNullOrEmpty() {
        // Arrange
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name)
                .IsNotNullOrEmpty()
            .Rule(x => x.Description)
                .IsNotNullOrEmpty());

        // Act
        var validationResult = validator.Validate(new TestEntity {
            Name = "",
            Description = null
        });

        // Assert
        Assert.That(validationResult.IsValid, Is.False);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(2));
        Assert.That(validationResult.Errors[0].ErrorMessage, Is.EqualTo("Name cannot be null or empty."));
        Assert.That(validationResult.Errors[1].ErrorMessage, Is.EqualTo("Description cannot be null or empty."));
    }

    [Test]
    public void StringValidator_HasMaxLength() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).HasMaxLength(5));

        var valid = validator.Validate(new TestEntity { Name = "Hello" });
        var invalid = validator.Validate(new TestEntity { Name = "HelloWorld" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name cannot exceed 5 characters."));
    }

    [Test]
    public void StringValidator_HasMinLength() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).HasMinLength(3));

        var valid = validator.Validate(new TestEntity { Name = "Test" });
        var invalid = validator.Validate(new TestEntity { Name = "Hi" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must be at least 3 characters."));
    }

    [Test]
    public void StringValidator_IsNotNullOrWhiteSpace() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).IsNotNullOrWhiteSpace());

        var valid = validator.Validate(new TestEntity { Name = "Test" });
        var invalid = validator.Validate(new TestEntity { Name = "   " });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name cannot be null or whitespace."));
    }

    [Test]
    public void StringValidator_MatchesRegex() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).MatchesRegex(@"^A.*Z$"));

        var valid = validator.Validate(new TestEntity { Name = "ABZ" });
        var invalid = validator.Validate(new TestEntity { Name = "BZ" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name is not in the correct format."));
    }

    [Test]
    public void StringValidator_DoesNotContain() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).DoesNotContain("bad"));

        var valid = validator.Validate(new TestEntity { Name = "goodstring" });
        var invalid = validator.Validate(new TestEntity { Name = "thisisbad" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name cannot contain 'bad'."));
    }

    [Test]
    public void StringValidator_Contains() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).Contains("ok"));

        var valid = validator.Validate(new TestEntity { Name = "lookok" });
        var invalid = validator.Validate(new TestEntity { Name = "fail" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must contain 'ok'."));
    }

    [Test]
    public void StringValidator_StartsWith() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).StartsWith("Pre"));

        var valid = validator.Validate(new TestEntity { Name = "Prefix" });
        var invalid = validator.Validate(new TestEntity { Name = "Suffix" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must start with 'Pre'."));
    }

    [Test]
    public void StringValidator_EndsWith() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).EndsWith("End"));

        var valid = validator.Validate(new TestEntity { Name = "TheEnd" });
        var invalid = validator.Validate(new TestEntity { Name = "Start" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must end with 'End'."));
    }

    [Test]
    public void StringValidator_IsEmail() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).IsEmail());

        var valid = validator.Validate(new TestEntity { Name = "test@example.com" });
        var invalid = validator.Validate(new TestEntity { Name = "notanemail" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must be a valid email address."));
    }

    [Test]
    public void StringValidator_IsGuid() {
        Validator<TestEntity> validator = Validator.For<TestEntity>(builder => builder
            .Rule(x => x.Name).IsGuid());

        var valid = validator.Validate(new TestEntity { Name = Guid.NewGuid().ToString() });
        var invalid = validator.Validate(new TestEntity { Name = "not-a-guid" });

        Assert.That(valid.IsValid, Is.True);
        Assert.That(invalid.IsValid, Is.False);
        Assert.That(invalid.Errors[0].ErrorMessage, Is.EqualTo("Name must be a valid GUID."));
    }
}