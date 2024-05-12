using SKEventBus;

namespace Events
{
  public class TestEvent3 : Event
  {
    public TestEvent3()
    {
    }

    public int TestId { get; set; }
    public string TestName { get; set; }
  }
}
