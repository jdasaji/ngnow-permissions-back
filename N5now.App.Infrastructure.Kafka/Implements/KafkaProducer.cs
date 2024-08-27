
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using N5now.App.Infrastructure.Kafka.Interfaces;
using System.Collections.Generic;

#nullable enable
namespace N5now.App.Infrastructure.Kafka.Implements
{
  public class KafkaProducer : IEventProducer
  {
    public kafkaSettings _kafkaSettings;

    public KafkaProducer(IOptions<kafkaSettings> options) => this._kafkaSettings = options.Value;

    public void Produce(string topic, string message)
    {
      ProducerConfig config = new ProducerConfig();
      config.BootstrapServers = this._kafkaSettings.Hostname + ":" + this._kafkaSettings.Port;
      using (IProducer<Null, string> producer = new ProducerBuilder<Null, string>((IEnumerable<KeyValuePair<string, string>>) config).Build())
      {
        Message<Null, string> message1 = new Message<Null, string>()
        {
          Value = message
        };
        producer.ProduceAsync(topic, message1).GetAwaiter().GetResult();
      }
    }
  }
}
