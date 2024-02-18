using Frank.WireFish;
using Frank.WireFish.PacketScroller;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWireFish(fishBuilder =>
{
    fishBuilder.AddPacketHandler<PacketCatogorizingHandler>();
});

builder.Services.AddSingleton<IPacketContainer, PacketContainer>();

var host = builder.Build();
host.Run();