using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable; // Add this if ConsumerConfig is in this namespace, otherwise adjust accordingly
using System.Text.Json;
using System.Text;

public class WalletStreamConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventPublisher _eventPublisher;

    public WalletStreamConsumer(IServiceProvider serviceProvider, IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
        _serviceProvider = serviceProvider;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var streamSystem = await StreamSystem.Create(new StreamSystemConfig());
        await streamSystem.CreateStream(new StreamSpec("wallet-stream")
        {
            MaxLengthBytes = 5_000_000_000
        });
        var consumer = await Consumer.Create(new ConsumerConfig(streamSystem, "wallet-stream")
        {
            OffsetSpec = new OffsetTypeNext(),
            MessageHandler = async (stream, _, _, message) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(message.Data.Contents);
                    var userCreated = JsonSerializer.Deserialize<UserCreatedEvent>(json);

                    if (userCreated != null)
                    {
                        // Create a new scope to resolve scoped services
                        using var scope = _serviceProvider.CreateScope();
                        var walletService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                        Console.WriteLine($"Creating wallet for User: {userCreated.UserId}");
                        await walletService.CreateWalletAsync(userCreated.UserId);
                        var userCreatedEvent = new UserActivateEvent
                        {
                            UserId = userCreated.UserId,
                            EventType = "UserActivate",
                            TargetService = "UserService"
                        };

                        await _eventPublisher.PublishAsync("user-confirm-stream", userCreatedEvent);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process message: {ex.Message}");
                }
            }

        });



    }
}