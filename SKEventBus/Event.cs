namespace SKEventBus
{
  public abstract class Event : IEvent
  {
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set;} = DateTime.Now;
    public string Source { get; set; }
  }
}
