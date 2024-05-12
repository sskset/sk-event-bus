
using Events;
using SKEventBus.EventBridge;

var eventBus = new EventBrdigeEventBus(
    Environment.GetEnvironmentVariable("_endpointId")
    );


await eventBus.PublishAsync(new TestEvent1
{
    TestId = 1,
    TestName = "Publish to aws eventbridge"
});

Console.WriteLine("Done");
Console.ReadKey();