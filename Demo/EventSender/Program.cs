
using Events;
using SKEventBus.ServiceBus;

var bus = new ServiceBusEventBus(Environment.GetEnvironmentVariable("_ServiceBusConnectionString"));
for (int i = 0; i < 10; i++)
{
  var @event = new TestEvent
  {
    TestId = i,
    TestName = $"Test - {i}"
  };

  await bus.PublishAsync(@event);

  Console.WriteLine($"Event: {@event.TestName} has been published.");

  Thread.Sleep(3000);
}

Console.WriteLine("Done");

