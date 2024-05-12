using SKEventBus;

namespace Events
{
  public class TestEvent1 : Event
  {
    public TestEvent1()
    {
    }

    public int TestId { get; set; }
    public string TestName { get; set; }
  }
}
