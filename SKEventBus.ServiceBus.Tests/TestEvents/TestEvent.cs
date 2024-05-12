
namespace SKEventBus.ServiceBus.Tests;

public class TestEvent : SKEventBus.IEvent
{
    public Guid EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Source { get; set; }
    public int TestId { get; set; } = 123;
}

public class TestEventHandler : SKEventBus.IEventHandler<TestEvent>
{
    public async Task HandleAsync(TestEvent @event)
    {
        Console.WriteLine(@event.TestId);
    }
}