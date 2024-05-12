namespace SKEventBus
{
  public interface IEventBus
  {
    void Register<TEvent, TEventHandler>()
        where TEvent : SKEventBus.IEvent
        where TEventHandler : SKEventBus.IEventHandler<TEvent>;

    void Deregister<TEvent, TEventHandler>()
        where TEvent : SKEventBus.IEvent
        where TEventHandler : SKEventBus.IEventHandler<TEvent>;

    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    Task StartListeningAsync();
  }
}