using GrpcStreamBenchmark.Producer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OpenTelemetry;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5126, o => o.Protocols = HttpProtocols.Http1AndHttp2);
    options.ListenLocalhost(7126, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();
builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.AddPrometheusExporter()
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddMeter(ServiceMetrics.Namespace);
    });

var app = builder.Build();

app.MapGrpcService<ProducerService>();
app.MapGet("/", () => Environment.ProcessId);

app.Run();
