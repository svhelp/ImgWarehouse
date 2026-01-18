using ImgWarehouse.Core.DTO;
using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class RecoursiveSplitProcessor : DirectoryProcessor
{
    public RecoursiveSplitProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override DirectoryData GetDirectoryData(string directoryPath)
    {
        var result = new DirectoryData
        {
            Path = directoryPath,
        };

        TraverseDirectories(directoryPath, (currentDirPath) =>
        {
            var currentDirData = GetSingleDirectoryData(currentDirPath, false, false);

            if (!currentDirData.Images.Any())
            {
                return;
            }

            result.ArchiveEntries.Add(currentDirPath);
            result.ContactLists.Add(currentDirData);
        });

        return result;
    }
}
