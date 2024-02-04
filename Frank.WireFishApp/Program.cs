using Frank.WireFish;
using Frank.WireFishApp;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPacketCaptureService();
builder.Services.AddHostedService<CaptureDataRunner>();

var host = builder.Build();
host.Run();