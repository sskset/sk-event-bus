using Amazon.SimpleNotificationService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace SKEventBus.SNS
{
  public class SNSEventBus : EventBus
  {
    private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

    public SNSEventBus(IAmazonSimpleNotificationService amazonSimpleNotificationService)
    {
      _amazonSimpleNotificationService = amazonSimpleNotificationService;
    }

    public override async Task PublishAsync<TEvent>(TEvent @event)
    {
      var eventName = GetEventName<TEvent>();

      var topic = await _amazonSimpleNotificationService.FindTopicAsync(eventName);
      var topicArn = topic?.TopicArn;
      if (string.IsNullOrEmpty(topicArn))
      {
        var createTopicResponse = await _amazonSimpleNotificationService.CreateTopicAsync(eventName);
        topicArn = createTopicResponse?.TopicArn;
      }

      var payload = JsonConvert.SerializeObject(@event);

      await _amazonSimpleNotificationService.PublishAsync(topicArn, payload);
    }

    public override Task StartListeningAsync()
    {
      throw new NotImplementedException();
    }
  }
}
