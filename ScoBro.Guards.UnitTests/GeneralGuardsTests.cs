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
}

public class GeneralGuardTestClass {
    public string Name { get; set; }
}
