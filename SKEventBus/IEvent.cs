namespace SKEventBus
{
    public interface IEvent
    {
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
    }
}