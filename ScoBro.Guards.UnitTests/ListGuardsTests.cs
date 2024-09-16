namespace ScoBro.Guards.UnitTests;
public class ListGuardsTests {
    [Test]
    public void HasMinimumCount_NullValue_Throws() {
        List<string> list = null;

        Assert.Throws<ArgumentNullException>(() => Guard.For(list).HasMinimumCount(1));
    }

    [Test]
    public void HasMinimumCount_ToFewItems_Throws() {
        List<string> list = [ "test", "test2" ];

        Assert.Throws<ArgumentException>(() => Guard.For(list).HasMinimumCount(3));
    }

    [Test]
    public void HasMinimumCount_ValidValue_Successful() {
        List<string> list = ["test", "test2"];

        List<string> testValue = Guard.For(list).HasMinimumCount(1);
        Assert.That(list, Is.EqualTo(testValue));
    }
}
