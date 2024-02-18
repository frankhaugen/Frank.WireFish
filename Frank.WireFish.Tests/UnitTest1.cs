using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Frank.Testing.TestBases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PacketDotNet;

using Xunit.Abstractions;

using ProtocolType = System.Net.Sockets.ProtocolType;

namespace Frank.WireFish.Tests;

public class WirefishTests(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddWireFish(wireFishBuilder => wireFishBuilder.AddPacketHandler<TestPacketHandler>());
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test1()
    {
        var message = "Hello World";
        outputHelper.WriteLine($"{message} on port 666");
        
        // Set up the server
        using var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 666));
        serverSocket.Listen(1);
        
        // Set up the client
        using var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 666));
        clientSocket.Send(Encoding.UTF8.GetBytes(message));
        
        // Wait for the packet handler to receive the packet
        await Task.Delay(1000);
        
        // Assert that the packet handler received the packet
        var packetHandler = Services.GetRequiredService<IPacketHandler>() as TestPacketHandler;
        Assert.NotNull(packetHandler);
        Assert.Single(packetHandler.Packets);
        var packet = packetHandler.Packets.First();
        Assert.Equal(message, Encoding.UTF8.GetString(packet.Packet.Extract<TcpPacket>().PayloadData));
    }

    private class TestPacketHandler : IPacketHandler
    {
        private readonly ConcurrentBag<DevicePacket> _packets = new();
        
        /// <inheritdoc />
        public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
        {
            _packets.Add(packet);
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public bool CanHandle(DevicePacket packet)
        {
            try
            {
                const int devilishPort = 666;
                var isDevilish = packet.Packet.Extract<TcpPacket>().DestinationPort == devilishPort || packet.Packet.Extract<TcpPacket>().SourcePort == devilishPort;
                var hasPayload = packet.Packet.Extract<TcpPacket>().PayloadData.Length > 0;
                return isDevilish && hasPayload;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public IEnumerable<DevicePacket> Packets => _packets;
    }
}