using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable; // Add this if ProducerConfig is in this namespace
using System.Text;
using System.Text.Json;
public class EventPublisher : IEventPublisher
{
    private readonly Task<StreamSystem> _streamSystemTask;

    public EventPublisher(Task<StreamSystem> streamSystemTask)
    {
        _streamSystemTask = streamSystemTask;
    }

    public async Task PublishAsync<T>(string streamName, T @event)
    {
        var streamSystem = await _streamSystemTask;
        var producer = await Producer.Create(new ProducerConfig(streamSystem, streamName));
        var message = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)));
        await producer.Send(message);
    }
}
