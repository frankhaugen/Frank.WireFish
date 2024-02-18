using Frank.Channels.DependencyInjection;
using Frank.WireFish;
using Frank.WireFishApp.FileWriting;
using Frank.WireFishApp.Irc;

using Microsoft.Extensions.Diagnostics.Metrics;

var builder = Host.CreateApplicationBuilder(args);
builder.Metrics.AddDebugConsole();
builder.Logging.AddDebug();

// Add the file writer
builder.Services.AddChannel<FileWriteRequest>();
builder.Services.AddHostedService<FileWriter>();


// Add the IRC packet service
builder.Services.AddChannel<IrcPacket>();
builder.Services.AddHostedService<IrcPacketService>();

// Add WireFish
builder.Services.AddWireFish(wireFishBuilder => wireFishBuilder.AddPacketHandler<IrcPacketHandler>());

var host = builder.Build();
host.Run();