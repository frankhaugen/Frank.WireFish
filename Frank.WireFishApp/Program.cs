using Frank.WireFish;
using Microsoft.Extensions.Diagnostics.Metrics;

var builder = Host.CreateApplicationBuilder(args);
builder.Metrics.AddDebugConsole();
builder.Logging.AddDebug();
builder.Services.AddPacketCaptureService();

var host = builder.Build();
host.Run();