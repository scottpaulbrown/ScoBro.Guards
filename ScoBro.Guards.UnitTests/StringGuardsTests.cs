namespace ScoBro.Guards.UnitTests;
public class StringGuardsTests {
    [Test]
    public void IsNotNullOrEmpty_NullValue_Throws() {
        string value = null;

        Assert.Throws<ArgumentNullException>(() => Guard.For(value).IsNotNullOrEmpty());
    }

    [Test]
    public void IsNotNullOrEmpty_ValidValue_Successful() {
        string value = "test";

        string testValue = Guard.For(value).IsNotNullOrEmpty();
        Assert.That(value, Is.EqualTo(testValue));
    }

    [Test]
    public void HasMaxLength_TooLong_Throws() {
        string? value = "123456";

        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.For(value).HasMaxLength(5));
    }

    [Test]
    public void HasMaxLength_ValidValue_Successful() {
        string value = "test";

        string testValue = Guard.For(value).HasMaxLength(10);
        Assert.That(value, Is.EqualTo(testValue));
    }
}
