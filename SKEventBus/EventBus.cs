namespace SKEventBus
{

    public abstract class EventBus : IEventBus
    {
        private Dictionary<string, EventSubscriptionInfo> _eventStore;

        public EventBus()
        {
            _eventStore = new Dictionary<string, EventSubscriptionInfo>();
        }

        public Dictionary<string, EventSubscriptionInfo> EventStore => _eventStore;
        public abstract Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
        public abstract Task SubscribeAsync();

        public virtual void Register<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;

            if (!_eventStore.ContainsKey(eventName))
            {
                _eventStore.Add(eventName, new EventSubscriptionInfo
                {
                    EventType = typeof(TEvent),
                    EventHandlerType = typeof(TEventHandler)
                });
            }
        }


        public class EventSubscriptionInfo
        {
            public Type EventType { get; set; }
            public Type EventHandlerType { get; set; }
        }
    }
}