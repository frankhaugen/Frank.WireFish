using System.Text;
using System.Threading.Channels;

namespace Frank.WireFishApp.FileWriting;

public class FileWriter(ChannelReader<FileWriteRequest> reader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (FileWriteRequest item in reader.ReadAllAsync(stoppingToken))
        {
            if (!item.FileInfo.Directory!.Exists) item.FileInfo.Directory.Create();
            if (!item.FileInfo.Exists) await item.FileInfo.Create().DisposeAsync();
            if (item.Append)
                await File.AppendAllTextAsync(item.FileInfo.FullName, item.Content + "\n", Encoding.UTF8, stoppingToken);
            else
                await File.WriteAllTextAsync(item.FileInfo.FullName, item.Content + "\n", Encoding.UTF8, stoppingToken);
        }
    }
}

