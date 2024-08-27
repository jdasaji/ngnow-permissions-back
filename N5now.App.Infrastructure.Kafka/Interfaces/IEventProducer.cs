
#nullable enable
namespace N5now.App.Infrastructure.Kafka.Interfaces
{
  public interface IEventProducer
  {
    void Produce(string topic, string message);
  }
}
