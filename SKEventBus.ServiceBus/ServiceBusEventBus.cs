
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SKEventBus.ServiceBus
{
  public class ServiceBusEventBus : EventBus
  {
    private readonly string _connectionString;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ServiceBusEventBus> _logger;

    public ServiceBusEventBus(string connectionString, IServiceProvider serviceProvider, ILogger<ServiceBusEventBus> logger)
    {
      _connectionString = connectionString;
      _serviceProvider = serviceProvider;
      _logger = logger;
    }

    public override async Task PublishAsync<TEvent>(TEvent @event)
    {
      //
      var topic = @event.GetType().Name;

      // Create topic if not exists
      var adminClient = new ServiceBusAdministrationClient(_connectionString);
      if (!await adminClient.TopicExistsAsync(topic))
      {
        await adminClient.CreateTopicAsync(topic);
      }

      // format message
      var payload = JsonConvert.SerializeObject(@event);
      var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(payload))
      {
        Subject = topic
      };

      // Publish message
      var client = new ServiceBusClient(_connectionString);
      var sender = client.CreateSender(topic);

      try
      {
        await sender.SendMessageAsync(message);
      }
      catch
      {
        throw;
      }
      finally
      {
        await sender.DisposeAsync();
        await client.DisposeAsync();
      }

    }

    public override async Task StartListeningAsync()
    {
      var adminClient = new ServiceBusAdministrationClient(_connectionString);
      var client = new ServiceBusClient(_connectionString);
      foreach (var @event in Events)
      {
        var topicName = @event.Key;

        // create subscription
        var subscriptionNamePrefix = GetSubscriptionNamePrefix(@event.Value.EventHandlerType);
        var subscriptionName = $"{subscriptionNamePrefix}_{@event.Key}";

        if (@event.Value.State == SubscriptionState.Subscribe)
        {
          // make sure topic exists
          if(!await adminClient.TopicExistsAsync(topicName))
          {
            await adminClient.CreateTopicAsync(topicName);
          }

          // make sure subscription exists
          if (!await adminClient.SubscriptionExistsAsync(topicName, subscriptionName))
          {
            await adminClient.CreateSubscriptionAsync(topicName, subscriptionName);
          }

          // create a processor that we use to process messages
          var processor = client.CreateProcessor(topicName, subscriptionName);

          // add handler to process messages
          processor.ProcessMessageAsync += MessageHandler;

          // add handler to process errors
          processor.ProcessErrorAsync += ErrorHandler;

          // listening
          await processor.StartProcessingAsync();
        }
        else if(@event.Value.State == SubscriptionState.Unsubscribe)
        {
          // for unsubscribed events, we get rid of the subscriptions only (keep topics)
          if(await adminClient.SubscriptionExistsAsync(topicName,subscriptionName))
          {
            await adminClient.DeleteSubscriptionAsync(topicName, subscriptionName);
          }
        }
      }
    }


    Task ErrorHandler(ProcessErrorEventArgs args)
    {
      _logger?.LogError("Error in processing message: {message} with exception: {exception}", args.EntityPath, args.Exception);
      return Task.CompletedTask;
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
      var eventName = args.Message.Subject;

      if (Events.TryGetValue(eventName, out var eventSubscriptionInfo))
      {
        var payload = Encoding.UTF8.GetString(args.Message.Body);
        var @event = JsonConvert.DeserializeObject(payload, eventSubscriptionInfo.EventType);

        var handlerType = eventSubscriptionInfo.EventHandlerType;
        var handler = _serviceProvider.GetService(eventSubscriptionInfo.EventHandlerType);
        
        var methodInfo = handlerType.GetMethod("HandleAsync");
        var task = (Task)methodInfo.Invoke(handler, new object[] { @event });

        await task;
      }

      await args.CompleteMessageAsync(args.Message);
    }

    private string GetSubscriptionNamePrefix(Type handlerType)
    {
      Assembly assembly = handlerType.Assembly;
      return $"sub_{assembly.GetName().Name.Replace(".", "_").ToLower()}";
    }
  }
}
