using ImgWarehouse.Core.Processors;
using ImgWarehouse.Logging;
using ImgWarehouse.Models;
using Microsoft.Extensions.Logging;

namespace ImgWarehouse.Core;

internal class MainProcessor
{
    public MainProcessor(ProcessorConfig config)
    {
        Config = config;
    }

    private ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<MainProcessor>();

    private DirectoryProcessorFactory DirectoryProcessorFactory { get; set; } = new DirectoryProcessorFactory();

    private ProcessorConfig Config { get; set; }

    public void Process(int contactListSize, int chunkSize)
    {
        string baseDirectory = Directory.GetCurrentDirectory();

        Logger.LogDebug($"Base Directory: {baseDirectory}");

        var processor = DirectoryProcessorFactory.CreateProcessor(Config);

        foreach (var subDir in Directory.GetDirectories(baseDirectory))
        {
            var relativePath = Path.GetRelativePath(baseDirectory, subDir);

            processor.ProcessDirectory(baseDirectory, relativePath);
        }
    }
}
