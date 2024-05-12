namespace SKEventBus.Exceptions
{
  [Serializable]
  public class EventDuplicatedException : Exception
  {
    public string EventName { get; }

    public EventDuplicatedException()
    {
    }

    public EventDuplicatedException(string message) : base(message)
    {
    }

    public EventDuplicatedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public EventDuplicatedException(string message, string eventName) : base(message)
    {
      EventName = eventName;
    }
  }
}
