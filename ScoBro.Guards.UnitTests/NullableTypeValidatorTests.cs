namespace ScoBro.Guards.UnitTests;

/// <summary>
/// Tests for nullable type validation support across all validation extensions.
/// </summary>
public class NullableTypeValidatorTests {
    
    #region Test Entities
    
    public class NullableStringEntity {
        public string? OptionalName { get; set; }
        public string? OptionalEmail { get; set; }
    }

    public class NullableIntEntity {
        public int? OptionalAge { get; set; }
        public int? OptionalCount { get; set; }
    }

    public class NullableLongEntity {
        public long? OptionalValue { get; set; }
    }

    public class NullableShortEntity {
        public short? OptionalValue { get; set; }
    }

    public class NullableDecimalEntity {
        public decimal? OptionalPrice { get; set; }
    }

    public class NullableDoubleEntity {
        public double? OptionalScore { get; set; }
    }

    public class NullableDateTimeEntity {
        public DateTime? OptionalDate { get; set; }
    }

    public class NullableGuidEntity {
        public Guid? OptionalId { get; set; }
    }
    
    #endregion

    #region Nullable String Tests

    [Test]
    public void NullableString_IsNotNullOrEmpty_NullValue_IsInvalid() {
        var validator = Validator.For<NullableStringEntity>(builder => builder
            .Rule(x => x.OptionalName).IsNotNullOrEmpty());

        var result = validator.Validate(new NullableStringEntity { OptionalName = null });

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("OptionalName cannot be null or empty."));
    }

    [TestCase(null, true)]
    [TestCase("", true)]
    [TestCase("Hello", true)]
    [TestCase("TooLongValue", false)]
    public void NullableString_HasMaxLength_NullIsValid(string? value, bool expectedValid) {
        var validator = Validator.For<NullableStringEntity>(builder => builder
            .Rule(x => x.OptionalName).HasMaxLength(5));

        var result = validator.Validate(new NullableStringEntity { OptionalName = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [TestCase(null, true)]
    [TestCase("test@example.com", true)]
    [TestCase("invalid-email", false)]
    public void NullableString_IsEmail_NullIsValid(string? value, bool expectedValid) {
        var validator = Validator.For<NullableStringEntity>(builder => builder
            .Rule(x => x.OptionalEmail).IsEmail());

        var result = validator.Validate(new NullableStringEntity { OptionalEmail = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [Test]
    public void NullableString_ChainedValidations_WithNull() {
        var validator = Validator.For<NullableStringEntity>(builder => builder
            .Rule(x => x.OptionalEmail)
                .HasMaxLength(100)
                .IsEmail());

        var result = validator.Validate(new NullableStringEntity { OptionalEmail = null });

        Assert.That(result.IsValid, Is.True);
    }

    #endregion

    #region Nullable Int Tests

    [TestCase(null, true)]
    [TestCase(5, true)]
    [TestCase(-1, false)]
    public void NullableInt_IsNotNegative_NullIsValid(int? value, bool expectedValid) {
        var validator = Validator.For<NullableIntEntity>(builder => builder
            .Rule(x => x.OptionalAge).IsNotNegative());

        var result = validator.Validate(new NullableIntEntity { OptionalAge = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [TestCase(null, true)]
    [TestCase(10, true)]
    [TestCase(0, false)]
    [TestCase(-5, false)]
    public void NullableInt_IsPositive_NullIsValid(int? value, bool expectedValid) {
        var validator = Validator.For<NullableIntEntity>(builder => builder
            .Rule(x => x.OptionalAge).IsPositive());

        var result = validator.Validate(new NullableIntEntity { OptionalAge = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [TestCase(null, true)]
    [TestCase(50, true)]
    [TestCase(18, true)]
    [TestCase(120, true)]
    [TestCase(17, false)]
    [TestCase(121, false)]
    public void NullableInt_IsInRange_NullIsValid(int? value, bool expectedValid) {
        var validator = Validator.For<NullableIntEntity>(builder => builder
            .Rule(x => x.OptionalAge).IsInRange(18, 120));

        var result = validator.Validate(new NullableIntEntity { OptionalAge = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [Test]
    public void NullableInt_ChainedValidations_WithNull() {
        var validator = Validator.For<NullableIntEntity>(builder => builder
            .Rule(x => x.OptionalCount)
                .IsPositive()
                .IsInRange(1, 100));

        var result = validator.Validate(new NullableIntEntity { OptionalCount = null });

        Assert.That(result.IsValid, Is.True);
    }

    #endregion

    #region Nullable Long Tests

    [TestCase(null, true)]
    [TestCase(100L, true)]
    [TestCase(-1L, false)]
    public void NullableLong_IsNotNegative_NullIsValid(long? value, bool expectedValid) {
        var validator = Validator.For<NullableLongEntity>(builder => builder
            .Rule(x => x.OptionalValue).IsNotNegative());

        var result = validator.Validate(new NullableLongEntity { OptionalValue = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    #endregion

    #region Nullable Short Tests

    [TestCase(null, true)]
    [TestCase((short)10, true)]
    [TestCase((short)-1, false)]
    public void NullableShort_IsNotNegative_NullIsValid(short? value, bool expectedValid) {
        var validator = Validator.For<NullableShortEntity>(builder => builder
            .Rule(x => x.OptionalValue).IsNotNegative());

        var result = validator.Validate(new NullableShortEntity { OptionalValue = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    #endregion

    #region Nullable Decimal Tests

    [TestCase(null, true)]
    [TestCase(10.50, true)]
    [TestCase(0, false)]
    [TestCase(-5.25, false)]
    public void NullableDecimal_IsPositive_NullIsValid(double? doubleValue, bool expectedValid) {
        var value = doubleValue.HasValue ? (decimal?)doubleValue.Value : null;
        var validator = Validator.For<NullableDecimalEntity>(builder => builder
            .Rule(x => x.OptionalPrice).IsPositive());

        var result = validator.Validate(new NullableDecimalEntity { OptionalPrice = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    [Test]
    public void NullableDecimal_IsInRange_NullIsValid() {
        var validator = Validator.For<NullableDecimalEntity>(builder => builder
            .Rule(x => x.OptionalPrice).IsInRange(0.01m, 999999.99m));

        var nullResult = validator.Validate(new NullableDecimalEntity { OptionalPrice = null });
        var validResult = validator.Validate(new NullableDecimalEntity { OptionalPrice = 100m });
        var invalidResult = validator.Validate(new NullableDecimalEntity { OptionalPrice = 0m });

        Assert.That(nullResult.IsValid, Is.True);
        Assert.That(validResult.IsValid, Is.True);
        Assert.That(invalidResult.IsValid, Is.False);
    }

    #endregion

    #region Nullable Double Tests

    [TestCase(null, true)]
    [TestCase(50.5, true)]
    [TestCase(-1.0, false)]
    public void NullableDouble_IsNotNegative_NullIsValid(double? value, bool expectedValid) {
        var validator = Validator.For<NullableDoubleEntity>(builder => builder
            .Rule(x => x.OptionalScore).IsNotNegative());

        var result = validator.Validate(new NullableDoubleEntity { OptionalScore = value });

        Assert.That(result.IsValid, Is.EqualTo(expectedValid));
    }

    #endregion

    #region Nullable DateTime Tests

    [Test]
    public void NullableDateTime_IsNotDefault_NullIsValid() {
        var validator = Validator.For<NullableDateTimeEntity>(builder => builder
            .Rule(x => x.OptionalDate).IsNotDefault());

        var nullResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = null });
        var validResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = DateTime.UtcNow });
        var invalidResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = default(DateTime) });

        Assert.That(nullResult.IsValid, Is.True);
        Assert.That(validResult.IsValid, Is.True);
        Assert.That(invalidResult.IsValid, Is.False);
    }

    [Test]
    public void NullableDateTime_IsNotInFuture_NullIsValid() {
        var validator = Validator.For<NullableDateTimeEntity>(builder => builder
            .Rule(x => x.OptionalDate).IsNotInFuture());

        var nullResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = null });
        var validResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = DateTime.UtcNow.AddDays(-1) });
        var invalidResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = DateTime.UtcNow.AddDays(1) });

        Assert.That(nullResult.IsValid, Is.True);
        Assert.That(validResult.IsValid, Is.True);
        Assert.That(invalidResult.IsValid, Is.False);
    }

    [Test]
    public void NullableDateTime_IsNotNullOrDefault_NullIsInvalid() {
        var validator = Validator.For<NullableDateTimeEntity>(builder => builder
            .Rule(x => x.OptionalDate).IsNotNullOrDefault());

        var nullResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = null });
        var defaultResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = default });
        var validResult = validator.Validate(new NullableDateTimeEntity { OptionalDate = DateTime.UtcNow });

        Assert.That(nullResult.IsValid, Is.False);
        Assert.That(defaultResult.IsValid, Is.False);
        Assert.That(validResult.IsValid, Is.True);
    }

    #endregion

    #region Nullable Guid Tests

    [Test]
    public void NullableGuid_IsNotEmpty_NullIsValid() {
        var validator = Validator.For<NullableGuidEntity>(builder => builder
            .Rule(x => x.OptionalId).IsNotEmpty());

        var nullResult = validator.Validate(new NullableGuidEntity { OptionalId = null });
        var validResult = validator.Validate(new NullableGuidEntity { OptionalId = Guid.NewGuid() });
        var invalidResult = validator.Validate(new NullableGuidEntity { OptionalId = Guid.Empty });

        Assert.That(nullResult.IsValid, Is.True);
        Assert.That(validResult.IsValid, Is.True);
        Assert.That(invalidResult.IsValid, Is.False);
    }

    #endregion

    #region Complex Scenarios

    [Test]
    public void NullableProperties_WithMultipleValidationRules() {
        var validator = Validator.For<NullableStringEntity>(builder => builder
            .Rule(x => x.OptionalEmail)
                .HasMaxLength(255)
                .IsEmail());

        // Null should pass all validations
        var nullResult = validator.Validate(new NullableStringEntity { OptionalEmail = null });
        Assert.That(nullResult.IsValid, Is.True);

        // Valid email should pass
        var validResult = validator.Validate(new NullableStringEntity { OptionalEmail = "test@example.com" });
        Assert.That(validResult.IsValid, Is.True);

        // Invalid email should fail
        var invalidResult = validator.Validate(new NullableStringEntity { OptionalEmail = "not-an-email" });
        Assert.That(invalidResult.IsValid, Is.False);

        // Too long email should fail
        var tooLongResult = validator.Validate(new NullableStringEntity { 
            OptionalEmail = new string('a', 250) + "@test.com" 
        });
        Assert.That(tooLongResult.IsValid, Is.False);
    }

    #endregion
}

