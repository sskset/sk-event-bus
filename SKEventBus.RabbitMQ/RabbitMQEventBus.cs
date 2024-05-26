using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace SKEventBus.RabbitMQ
{
  public class RabbitMQEventBus : EventBus
  {
    private readonly IConnectionFactory _connFactory;

    public RabbitMQEventBus(IConnectionFactory connFactory)
    {
      _connFactory = connFactory;
    }

    public override Task PublishAsync<TEvent>(TEvent @event)
    {
      using (var conn = _connFactory.CreateConnection())
      {
        using (var channel = conn.CreateModel())
        {
          channel.ExchangeDeclare(exchange: "topic_exchange", type: ExchangeType.Topic);

          var routingKey = GetEventName<TEvent>();
          var message = JsonConvert.SerializeObject(@event);
          var payload = Encoding.UTF8.GetBytes(message);

          channel.BasicPublish(
            exchange: "topic_exchange",
            routingKey: routingKey,
            basicProperties: null,
            body: payload);
        }
      }

      return Task.CompletedTask;
    }

    public override Task StartListeningAsync()
    {
      throw new NotImplementedException();
    }
  }
}
