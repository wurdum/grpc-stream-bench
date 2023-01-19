using GrpcStreamBenchmark.Consumer;
using GrpcStreamBenchmark.Core;
using OpenTelemetry;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddHostedService<ConsumerService>();
builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.AddPrometheusExporter()
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddMeter(ServiceMetrics.Namespace);
    });

builder.Services.AddGrpcClient<RecordProducer.RecordProducerClient>(o =>
{
    o.Address = new(builder.Configuration.GetValue<string>("ProducerUrl")!);
    o.ChannelOptionsActions.Add(ch => ch.HttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
});

var app = builder.Build();

app.MapGet("/", () => Environment.ProcessId);

app.Run();
