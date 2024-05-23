using System;
using System.Threading.Tasks;
using Xunit;

namespace SKEventBus.ServiceBus.Tests
{

  public class ServiceBusEventBusTests
  {
    private readonly ServiceBusEventBus _eventBus;

    public ServiceBusEventBusTests() => _eventBus = new ServiceBusEventBus(
      Environment.GetEnvironmentVariable("_ServiceBusConnectionString"),
      null,
      null);

    [Fact]
    public async Task Should_Publish_Message()
    {
      await _eventBus.PublishAsync(new TestEvent());
    }

    // [Fact]
    // public async Task Should_Subscribe_Event()
    // {
    //     _eventBus.Register<TestEvent, TestEventHandler>();
    //     await _eventBus.PublishAsync(new TestEvent());
    //     await _eventBus.SubscribeAsync();
    // }
  }
}