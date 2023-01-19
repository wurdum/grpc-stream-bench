using System.Diagnostics.Metrics;
using GrpcStreamBenchmark.Core;

namespace GrpcStreamBenchmark.Consumer;

public static class ServiceMetrics
{
    public const string Namespace = "GrpcStreamBenchmark";
    private static readonly Meter Meter = new(Namespace);
    private static readonly Counter<long> ConsumerReceivedMessages = Meter.CreateCounter<long>("consumer_received_messages", "total");
    private static readonly Histogram<long> ConsumerReceivedMessageLatency = Meter.CreateHistogram<long>("consumer_received_message_latency", "ms");

    public static void RecordConsumed(Record record)
    {
        ConsumerReceivedMessages.Add(1);
        ConsumerReceivedMessageLatency.Record(Math.Abs(record.Timestamp - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    }
}
