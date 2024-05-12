using SKEventBus;

namespace Events
{
  public class TestEvent2 : Event
  {
    public TestEvent2()
    {
    }

    public int TestId { get; set; }
    public string TestName { get; set; }
  }
}
