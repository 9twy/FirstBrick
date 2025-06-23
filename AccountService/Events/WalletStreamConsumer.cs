using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable; // Add this if ConsumerConfig is in this namespace, otherwise adjust accordingly
using System.Text.Json;
using System.Text;

public class WalletStreamConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public WalletStreamConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var streamSystem = await StreamSystem.Create(new StreamSystemConfig());
        await streamSystem.CreateStream(new StreamSpec("user-confirm-stream")
        {
            MaxLengthBytes = 5_000_000_000
        });
        var consumer = await Consumer.Create(new ConsumerConfig(streamSystem, "user-confirm-stream")
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
                        var userServices = scope.ServiceProvider.GetRequiredService<IUserService>();

                        Console.WriteLine($"activate Account for User: {userCreated.UserId}");
                        await userServices.ActivateUserAsync(userCreated.UserId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process message: {ex.Message}");
                }
            }
        });
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }



    }
}