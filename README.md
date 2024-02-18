# Frank.WireFish
A network traffic capture library, not like, but still reminiscent of WireShark, hence the name, its a play on words, get it? WireFish? WireShark? No? Ok, I'll stop now.

## What is it?

WireFish is a network traffic capture library, it is designed to capture network traffic and do whatever you want with it. 
It is not a network protocol analyzer, it is not a network protocol sniffer, it is not a network protocol anything, it is a 
network traffic capture library. It is designed to capture network traffic and do whatever you want with it by registering 
dependencies and then using the library to capture network traffic and pass it to your dependencies in a way that is 
intuitive to anyone who are familiar with the .NET dependency injection.

## How do I use it?

WireFish is designed to be used in a .NET Core application, by registering dependencies in the `Startup.cs`/`Program.cs` 
file and then using the library to capture network traffic and pass it to your dependencies. Under the hood, WireFish uses 
Channels to pass on the captured network traffic to your dependencies, so you can use the library in a non-blocking way.

### How do I install it?

You can install WireFish from NuGet by running the following command in the Package Manager Console:

```
Install-Package Frank.WireFish
```

Or by running the following command in the .NET Core CLI:

```
dotnet add package Frank.WireFish
```

### How do I use it in my application?

First, you need to register the dependencies in the `Startup.cs`/`Program.cs` file. You can do this by adding something like the following:

```csharp
// Add the IRC packet service
builder.Services.AddChannel<IrcPacket>();
builder.Services.AddHostedService<IrcPacketService>();

// Add WireFish
builder.Services.AddWireFish(wireFishBuilder => wireFishBuilder.AddPacketHandler<IrcPacketHandler>());
```

Then, you need to create a packet handler that implements the `IPacketHandler` interface. You can do this by adding something like the following:

```csharp
public class IrcPacketHandler : IPacketHandler
{
    public async Task HandleAsync(DevicePacket packet)
    {
        // Do something with the packet
    }
    
    public bool CanHandle(DevicePacket packet)
    {
        // Return true if the packet can be handled by this handler
        // IMPORTANT: This should be as fast as possible, because it is called for every packet
    }
}
```

IPacketHandler.cs
```csharp
public interface IPacketHandler
{
    Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken);
    
    bool CanHandle(DevicePacket packet);
}
```

## What can I do with it?

You can use WireFish to capture network traffic and do whatever you want with it. If you are trying to build a network 
protocol analyzer, you can use WireFish to capture network traffic and then analyze it. If you are trying to build a network 
protocol sniffer, you can use WireFish to capture network traffic and then sniff it. If you are trying to build a network 
protocol anything, you can use WireFish to capture network traffic and then do whatever you want with it. Its however NOT a 
server, nor a client, and is on its own not a security tool, but it can be used to build security tools.

A good usage example is to use WireFish to make sure a network protocol is working as expected, or to see if your 
application is unnecessarily sending data over the network. You can also use WireFish to build a network protocol analyzer, 
or maybe a visualizer for network traffic.

## What can't I do with it?

Without changes or extension anything "hacky" or "unethical" is not possible, and it is not designed to be used for anything 
"hacky", but it can be used to build security tools, its however not intended for such a purpose so there is no guarantees 
that it will work for such a purpose at this time or any time in the future; e.g. security tools and network communication 
integrity tools, might flag the library as a security risk or packages as "sniffed" or "tampered with". So, if you are going 
to use it for such a purpose, you should be aware of the risks.

## What is the future of WireFish?

There is no roadmap for WireFish, its basically done, and there is no plans to add any new features, but it will be kept up 
to date, and any bugs will be fixed. If you have any feature requests, you can open an issue on the GitHub repository, but 
most likely it will not be added, because the library is designed to allow you to add your own behavior.

## Legal heads up

In some jurisdictions, it may be illegal to capture network traffic, or your school or employer may have a policy against 
such. Be careful and make sure you are allowed to use this kind of behavior before you use it. Also, general privacy laws 
might apply if you are capturing network traffic that is not your own. Most likely, this library will not cross any lines, 
but the maintainer of this library is not a lawyer, and even if they were, they would not be aware of the laws in your 
particular matrix of jurisdictions and regulations.

## License

This library is licensed under the MIT license. You can find the license in the `LICENSE` file in the root of the repository.

## Contributing

If you want to contribute to WireFish, you can open a pull request on the GitHub repository. Please make an issue before you 
make a pull request, so we can discuss the changes before you make them.