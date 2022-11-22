using System.Reflection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var executingAssembly = Assembly.GetExecutingAssembly().GetName();
var serviceName = executingAssembly.Name ?? "MyService";
var serviceVersion = executingAssembly.Version?.ToString() ?? "1.0.0";

builder.Services
    .AddOpenTelemetryMetrics(otBuilder => otBuilder
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName, serviceVersion: serviceVersion))
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:4317");
            opt.Protocol = OtlpExportProtocol.Grpc;
        })
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();