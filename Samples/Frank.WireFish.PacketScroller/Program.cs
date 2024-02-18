using Frank.WireFish;
using Frank.WireFish.PacketScroller;

using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWireFish(fishBuilder =>
{
    fishBuilder.AddPacketHandler<PacketCatogorizingHandler>();
});

builder.Services.AddSingleton<IPacketContainer, PacketContainer>();
builder.Services.AddSingleton<IAnsiConsole, MyAnsiConsole>();

var host = builder.Build();
host.Run();