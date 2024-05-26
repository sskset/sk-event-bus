using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Http;
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

      var topicArn = await GetTopicArnAsync(eventName);

      var payload = JsonConvert.SerializeObject(@event);

      var req = new PublishRequest
      {
        TopicArn = topicArn,
        Message = payload,
        Subject = eventName,
      };

      var response = await _amazonSimpleNotificationService.PublishAsync(topicArn, payload);
    }

    public override Task StartListeningAsync()
    {
      throw new NotImplementedException();
    }

    private async Task<string> GetTopicArnAsync(string topicName)
    {
      var topic = await _amazonSimpleNotificationService.FindTopicAsync(topicName);
      if (topic == null)
      {
        var res = await _amazonSimpleNotificationService.CreateTopicAsync(topicName);

        return res.TopicArn;
      }

      return topic.TopicArn;
    }

  }
}
