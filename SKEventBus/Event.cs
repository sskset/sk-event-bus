using System;

namespace SKEventBus
{
  public interface IEvent
  {
    public Guid EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Source { get; set; }
  }

  public abstract class Event : IEvent
  {
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set;} = DateTime.Now;
    public string Source { get; set; }
  }
}
