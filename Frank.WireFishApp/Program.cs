using Frank.WireFish;
using Frank.WireFishApp;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddDebug();
builder.Services.AddPacketCaptureService();
builder.Services.AddHostedService<CaptureDataRunner>();

var host = builder.Build();
host.Run();