using ImgWarehouse.Logging;
using ImgWarehouse.Models;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace ImgWarehouse.Core;

internal class Archiver
{
    private ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<Archiver>();

    public void ArchiveDirectory(ArchiveConfig config, string directoryPath)
    {
        var outputArchive = $"{directoryPath}.zip";

        if (File.Exists(outputArchive))
        {
            if (config.Forced)
            {
                Logger.LogWarning("Archive already exists for the directory, removing.");

                File.Delete(outputArchive);
            }
            else
            {
                Logger.LogWarning("Archive already exists for the directory, skipping.");

                return;
            }
        }

        Logger.LogDebug("Archiving directory");

        ZipFile.CreateFromDirectory(directoryPath, outputArchive, CompressionLevel.Optimal, includeBaseDirectory: false);

        Logger.LogInformation("Directory archived");
    }
}
