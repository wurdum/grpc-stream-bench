using GrpcStreamBenchmark.Consumer;
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

var app = builder.Build();

app.MapGet("/", () => Environment.ProcessId);

app.Run();
