using Amazon.SimpleNotificationService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SKEventBus.SNS.Tests
{

  public class SNSEventBusTests
  {
    private readonly IEventBus _eventBus;

    public SNSEventBusTests()
    {
      var snsClient = new AmazonSimpleNotificationServiceClient();
      _eventBus = new SNSEventBus(snsClient);
    }

    public class UserCreaetdEvent : Event
    {
      public Guid UserId { get; set; } = Guid.NewGuid();
      public string UserName { get; set; }
    }

    [Fact]
    public async Task Test_PublishAsync()
    {
      var testEvent = new UserCreaetdEvent
      {
        UserId = Guid.NewGuid(),
        UserName = "TestUser1"
      };

      await _eventBus.PublishAsync(testEvent);
    }
  }
}