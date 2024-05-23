using System.Threading.Tasks;

namespace SKEventBus
{
  public interface IEventHandler<TEvent> where TEvent : IEvent
  {
    Task HandleAsync(TEvent @event);
  }
}