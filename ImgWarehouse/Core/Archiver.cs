using ImgWarehouse.Logging;
using ImgWarehouse.Models;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace ImgWarehouse.Core
{
    internal class Archiver
    {
        private ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<Archiver>();

        public void ArchiveDirectory(ArchiveConfig config, string directoryPath)
        {
            if (config.Skip)
            {
                return;
            }

            if (File.Exists($"{directoryPath}.zip"))
            {
                if (config.Forced)
                {
                    Logger.LogWarning($"Archive already exists for directory: {directoryPath}, removing.");

                    File.Delete($"{directoryPath}.zip");
                }
                else
                {
                    Logger.LogWarning($"Archive already exists for directory: {directoryPath}, skipping.");

                    return;
                }
            }

            Logger.LogDebug($"Archiving directory: {directoryPath}");

            ZipFile.CreateFromDirectory(directoryPath, $"{directoryPath}.zip", CompressionLevel.Optimal, includeBaseDirectory: false);

            Logger.LogInformation($"Directory archived: {directoryPath}.zip");
        }
    }
}
