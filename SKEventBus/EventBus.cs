using SKEventBus.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SKEventBus
{

  public abstract class EventBus : IEventBus
  {
    private Dictionary<string, EventSubscriptionInfo> _events;

    public EventBus()
    {
      _events = new Dictionary<string, EventSubscriptionInfo>();
    }

    public Dictionary<string, EventSubscriptionInfo> Events => _events;

    public abstract Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    public abstract Task StartListeningAsync();

    protected string GetEventName<TEvent>() where TEvent : IEvent
    {
      return typeof(TEvent).Name;
    }

    protected string GetEventName(Type t)
    {
      return t.Name;
    }

    public virtual void Register<TEvent, TEventHandler>()
        where TEvent : IEvent
        where TEventHandler : IEventHandler<TEvent>
    {
      var eventName = GetEventName<TEvent>();

      if (!_events.ContainsKey(eventName))
      {
        _events.Add(eventName, new EventSubscriptionInfo
        {
          EventType = typeof(TEvent),
          EventHandlerType = typeof(TEventHandler),
          State = SubscriptionState.Subscribe
        });
      }
      else
      {
        throw new EventDuplicatedException("This event has been handled already.", eventName);
      }
    }

    public virtual void Deregister<TEvent, TEventHandler>() 
      where TEvent : IEvent
      where TEventHandler: IEventHandler<TEvent>
    {
      var eventName = GetEventName<TEvent>();

      if (!_events.ContainsKey(eventName))
      {
        _events.Add(eventName, new EventSubscriptionInfo
        {
          EventType = typeof(TEvent),
          EventHandlerType = typeof(TEventHandler),
          State = SubscriptionState.Unsubscribe
        });
      }
      else
      {
        throw new Exceptions.EventDuplicatedException("This event has been handled already.", eventName);
      }
    }

    public class EventSubscriptionInfo
    {
      public Type EventType { get; set; }
      public Type EventHandlerType { get; set; }
      public SubscriptionState State { get; set; } = SubscriptionState.Subscribe;
    }

    public enum SubscriptionState
    {
      Subscribe,
      Unsubscribe
    }
  }
}