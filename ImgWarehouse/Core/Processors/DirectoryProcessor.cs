using ImgWarehouse.Core.DTO;
using ImgWarehouse.Logging;
using ImgWarehouse.Models;
using Microsoft.Extensions.Logging;

namespace ImgWarehouse.Core.Processors;

internal abstract class DirectoryProcessor
{
    protected static readonly string[] SupportedImageExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp"];

    public DirectoryProcessor(ProcessorConfig config)
    {
        Config = config;
    }

    protected ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<MainProcessor>();

    private ProcessorConfig Config { get; set; }

    private ContactList ContactList { get; set; } = new ContactList();

    private Archiver Archiver { get; set; } = new Archiver();

    public void ProcessDirectory(string baseDirectory, string relativePath)
    {
        using var directoryScope = Logger.BeginScope(relativePath);

        var directoryPath = Path.Combine(baseDirectory, relativePath);

        Logger.LogDebug("Processing directory");

        var dirData = GetDirectoryData(directoryPath);

        foreach (var contactListData in dirData.ContactLists)
        {
            ContactList.Create(contactListData, baseDirectory);
        }

        if (!Config.ArchiveConfig.Skip)
        {
            foreach (var archiveEntry in dirData.ArchiveEntries)
            {
                Archiver.ArchiveDirectory(Config.ArchiveConfig, baseDirectory, archiveEntry);
            }
        }

        Logger.LogInformation("Successfully processed");
    }

    internal abstract DirectoryData GetDirectoryData(string directoryPath);

    protected ContactListData GetSingleDirectoryData(string directoryPath, bool subDirWarning, bool noImgWarning)
    {
        var images = Directory.EnumerateFiles(directoryPath)
            .Where(file => SupportedImageExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
            .ToList();

        if (noImgWarning && !images.Any())
        {
            Logger.LogWarning("No images found images in the directory");
        }

        if (subDirWarning && Directory.EnumerateDirectories(directoryPath).Any())
        {
            Logger.LogWarning("Directory contains subdirectories");
        }

        return new ContactListData()
        {
            Name = directoryPath,
            Images = images
        };
    }

    protected void TraverseDirectories(string directoryPath, Action<string> action)
    {
        action(directoryPath);

        foreach (var subDir in Directory.GetDirectories(directoryPath))
        {
            TraverseDirectories(subDir, action);
        }
    }
}
