public interface IEventPublisher
{
    Task PublishAsync<T>(string streamName, T @event);
}