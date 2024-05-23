using Amazon.EventBridge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SKEventBus.EventBridge
{
  public class EventBrdigeEventBus : EventBus
    {
        private readonly string _endpointId;

        public EventBrdigeEventBus(string endpointId)
        {
            _endpointId = endpointId;
        }

        public override async Task PublishAsync<TEvent>(TEvent @event)
        {
            var client = new AmazonEventBridgeClient();
            var response = await client.PutEventsAsync(
                new Amazon.EventBridge.Model.PutEventsRequest
                {
                    Entries = new List<Amazon.EventBridge.Model.PutEventsRequestEntry>
                    {
                        new Amazon.EventBridge.Model.PutEventsRequestEntry
                        {
                            Detail = JsonConvert.SerializeObject(@event),
                            DetailType = @event.GetType().Name
                        }
                    },
                    EndpointId = _endpointId,
                });
        }

        public override Task StartListeningAsync()
        {
            // the EventBridge doesn't support pub/sub pattern
            // the consumers have to be setup in the `Targets`
            // check aws documentation to figure out how to configure `Targets`
            throw new NotImplementedException();
        }
    }
}
