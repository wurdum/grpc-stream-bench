using Grpc.Core;
using GrpcStreamBenchmark.Core;

namespace GrpcStreamBenchmark.Consumer;

public class ConsumerService : BackgroundService
{
    private readonly RecordProducer.RecordProducerClient _client;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(RecordProducer.RecordProducerClient client, ILogger<ConsumerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stream = _client.GetRecords(new(), cancellationToken: stoppingToken);

                _logger.LogInformation("Consumer started.");

                await foreach (var record in stream.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    ServiceMetrics.RecordConsumed(record);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                _logger.LogInformation("Consumer cancelled.");
                break;
            }
            catch (OperationCanceledException oce) when (oce.CancellationToken == stoppingToken)
            {
                // Service stopped.
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer failed. Reconnecting in 2s.");
                await Task.Delay(2000, default);
            }
        }

        _logger.LogInformation("Consumer stopped.");
    }
}
