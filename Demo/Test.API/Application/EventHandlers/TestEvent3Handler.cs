using Events;
using SKEventBus;
using Test.API.Models;

namespace Test.API.Application.EventHandlers
{
  public class TestEvent3Handler : IEventHandler<TestEvent3>
  {

    private readonly ITestDbContext _testDbContext;

    public TestEvent3Handler(ITestDbContext testDbContext)
    {
      _testDbContext = testDbContext;
    }

    public Task HandleAsync(TestEvent3 @event)
    {
     _testDbContext.Add(@event.TestName);
      return Task.CompletedTask;
    }
  }
}
