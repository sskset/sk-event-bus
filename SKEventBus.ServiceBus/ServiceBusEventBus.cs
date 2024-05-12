
using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;

namespace SKEventBus.ServiceBus
{
    public class ServiceBusEventBus : SKEventBus.EventBus
    {
        private readonly string _connectionString;

        public ServiceBusEventBus(string connectionString)
        {
            _connectionString = connectionString;
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
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(payload));

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

        public override async Task SubscribeAsync()
        {
            var client = new ServiceBusClient(_connectionString);
            foreach(var @event in EventStore)
            {
                // create a processor that we use to process messages
                var processor = client.CreateProcessor(topicName: @event.Key, subscriptionName: $"sub_{@event.Key}");

                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process errors
                processor.ProcessErrorAsync += ErrorHandler;

                // listening
                await processor.StartProcessingAsync();
            }
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var eventName = args.Message.Subject;
            var payload = Encoding.UTF8.GetString(args.Message.Body);

            if(EventStore.TryGetValue(eventName, out var eventSubscriptionInfo))
            {
                var handlerType = eventSubscriptionInfo.EventHandlerType;
                var handler = Activator.CreateInstance(handlerType);

                var methodInfo = handlerType.GetMethod("HandleAsync");
                var task = (Task)methodInfo.Invoke(handler, new object[]{payload});

                await task;
            }

            await args.CompleteMessageAsync(args.Message);
        }
    }
}
