namespace Test.API.Models
{
  public interface ITestDbContext
  {
    IEnumerable<string> GetTests();

    void Add(string test);
  }

  public class InMemoryTestDbContext : ITestDbContext
  {
    private readonly List<string> _tests;

    public InMemoryTestDbContext()
    {
      _tests = new List<string>();
    }

    public void Add(string test)
    {
      _tests.Add(test);
    }

    public IEnumerable<string> GetTests()
    {
      return _tests;
    }
  }
}
