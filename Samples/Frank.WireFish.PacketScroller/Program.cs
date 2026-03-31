using Frank.Mapping;
using Frank.WireFish;
using Frank.WireFish.PacketScroller;
using Frank.WireFish.PacketScroller.Data;

using Realms;

using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWireFish(fishBuilder =>
{
    fishBuilder.AddPacketHandler<PacketCatogorizingHandler>();
    fishBuilder.AddPacketHandler<SaveDataHandler>();
});

builder.Services.AddSingleton<IPacketContainer, PacketContainer>();
builder.Services.AddSingleton<IAnsiConsole, MyAnsiConsole>();

builder.Services.AddMappingDefinition<DevicePacket, DevicePacketEntity, DevicePacketMapping>();
builder.Services.AddRealmService(new RealmConfiguration(Path.Combine(AppContext.BaseDirectory, "data.realm")));
builder.Services.AddRepository<DevicePacketEntity>();

var host = builder.Build();
host.Run();