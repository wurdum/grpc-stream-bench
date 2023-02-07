using System.Runtime.InteropServices;
using GrpcStreamBenchmark.Producer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OpenTelemetry;
using OpenTelemetry.Metrics;

foreach (var env in Environment.GetEnvironmentVariables())
{
    Console.WriteLine(env.ToString());
}

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    // Setup a HTTP/2 endpoint without TLS to run on Mac.
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5126, o => o.Protocols = HttpProtocols.Http1AndHttp2);
        options.ListenLocalhost(7126, o => o.Protocols = HttpProtocols.Http2);
    });
}

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.AddPrometheusExporter()
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddMeter(ServiceMetrics.Namespace);
    });

var app = builder.Build();

app.MapGrpcReflectionService();
app.MapGrpcService<ProducerService>();

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapGet("/", () => Environment.ProcessId);

app.Run();
