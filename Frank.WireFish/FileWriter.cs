using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

namespace Frank.WireFish;

public class FileWriter(ChannelReader<Tuple<FileInfo, string>> reader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            if (!item.Item1.Directory!.Exists) item.Item1.Directory.Create();
            if (!item.Item1.Exists) await item.Item1.Create().DisposeAsync();
            await File.AppendAllTextAsync(item.Item1.FullName, item.Item2 + "\n", Encoding.UTF8, stoppingToken);
        }
    }
}