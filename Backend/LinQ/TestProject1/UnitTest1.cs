using LinQ;

namespace TestProject1;

public class UnitTest1
{
    private Dodawacz d = new ();
    [Fact]
    public void Test1()
    {
        Assert.True(d.Dodaj(1, 2) == 3);
    }
}