namespace GrpcStreamBenchmark.Consumer;

public class ConsumerService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}
