using Events;
using SKEventBus;
using Test.API.Models;

namespace Test.API.Application.EventHandlers
{
  public class TestEvent2Handler : IEventHandler<TestEvent2>
  {

    private readonly ITestDbContext _testDbContext;

    public TestEvent2Handler(ITestDbContext testDbContext)
    {
      _testDbContext = testDbContext;
    }

    public Task HandleAsync(TestEvent2 @event)
    {
     _testDbContext.Add(@event.TestName);
      return Task.CompletedTask;
    }
  }
}
