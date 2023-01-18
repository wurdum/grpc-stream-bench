using Grpc.Core;

namespace GrpcStreamBenchmark.Producer;

public class ProducerService : RecordProducer.RecordProducerBase
{
    public override Task GetRecords(RecordStreamRequest request, IServerStreamWriter<Record> responseStream, ServerCallContext context)
    {
        return base.GetRecords(request, responseStream, context);
    }
}
