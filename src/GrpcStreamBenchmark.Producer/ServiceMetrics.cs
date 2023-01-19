using System.Diagnostics.Metrics;
using GrpcStreamBenchmark.Core;

namespace GrpcStreamBenchmark.Producer;

public static class ServiceMetrics
{
    public const string Namespace = "GrpcStreamBenchmark";
    private static readonly Meter Meter = new(Namespace);
    private static readonly Counter<long> ProducerMessagesSentCount = Meter.CreateCounter<long>("producer_messages_sent", "total");
    private static readonly Histogram<long> ProducerMessageSendingLatency = Meter.CreateHistogram<long>("producer_message_sending_latency", "ms");

    public static void RecordProduced(Record record)
    {
        ProducerMessagesSentCount.Add(1);
        ProducerMessageSendingLatency.Record(Math.Abs(record.Timestamp - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    }
}
