using Grpc.Core;
using GrpcStreamBenchmark.Core;

namespace GrpcStreamBenchmark.Producer;

public class ProducerService : RecordProducer.RecordProducerBase
{
    public override async Task GetRecords(RecordStreamRequest request, IServerStreamWriter<Record> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await responseStream.WriteAsync(Record.New());

            await Task.Delay(1000, default);
        }
    }
}
