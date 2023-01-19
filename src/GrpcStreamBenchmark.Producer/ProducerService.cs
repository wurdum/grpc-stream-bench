using Grpc.Core;
using GrpcStreamBenchmark.Core;

namespace GrpcStreamBenchmark.Producer;

public class ProducerService : RecordProducer.RecordProducerBase
{
    private readonly ILogger<ProducerService> _logger;

    public ProducerService(ILogger<ProducerService> logger)
    {
        _logger = logger;
    }

    public override async Task GetRecords(RecordStreamRequest request, IServerStreamWriter<Record> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Producer started.");

        while (!context.CancellationToken.IsCancellationRequested)
        {
            var record = Record.New();

            await responseStream.WriteAsync(record);
            ServiceMetrics.RecordProduced(record);
        }
    }
}
