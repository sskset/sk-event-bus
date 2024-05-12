using Events;
using SKEventBus;
using Test.API.Models;

namespace Test.API.Application.EventHandlers
{
  public class TestEvent1Handler : IEventHandler<TestEvent1>
  {

    private readonly ITestDbContext _testDbContext;

    public TestEvent1Handler(ITestDbContext testDbContext)
    {
      _testDbContext = testDbContext;
    }

    public Task HandleAsync(TestEvent1 @event)
    {
     _testDbContext.Add(@event.TestName);
      return Task.CompletedTask;
    }
  }
}
