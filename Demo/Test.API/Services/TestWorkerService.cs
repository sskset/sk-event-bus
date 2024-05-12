
using SKEventBus;

namespace Test.API.Services
{
  public class TestWorkerService : BackgroundService
  {
    private readonly IEventBus _eventBus;

    public TestWorkerService(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await _eventBus.StartListeningAsync();
    }
  }
}
