using Events;
using Microsoft.AspNetCore.Mvc;
using SKEventBus;
using Test.API.Models;

namespace Test.API.Controllers
{
  [ApiController]
  [Route("tests")]
  public class TestController : ControllerBase
  {
    private readonly ITestDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public TestController(ITestDbContext dbContext, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _eventBus = eventBus;
    }

    [HttpGet] 
    public IActionResult Index()
    {
      return Ok(_dbContext.GetTests());
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage()
    {

      await _eventBus.PublishAsync(new TestEvent1
      {
        EventId = Guid.NewGuid(),
        TestId = 1,
        TestName = "test - 1"
      });

      await _eventBus.PublishAsync(new TestEvent2
      {
        EventId = Guid.NewGuid(),
        TestId = 2,
        TestName = "test - 2"
      });

      await _eventBus.PublishAsync(new TestEvent3
      {
        EventId = Guid.NewGuid(),
        TestId = 3,
        TestName = "test - 3"
      });

      return Created();
    }
  }
}
