using Frank.WireFish;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddDebug();
builder.Services.AddPacketCaptureService();
// builder.Services.AddSqliteDataStorage<EthernetPacket>(new ConfigurationManager());

var host = builder.Build();
host.Run();