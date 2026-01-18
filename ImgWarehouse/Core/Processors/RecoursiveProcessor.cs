using ImgWarehouse.Core.DTO;
using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class RecoursiveProcessor : DirectoryProcessor
{
    public RecoursiveProcessor(ProcessorConfig config) : base(config)
    {
    }

    internal override DirectoryData GetDirectoryData(string directoryPath)
    {
        var result = new DirectoryData
        {
            Path = directoryPath,
        };

        var mergedDirectoryData = new List<string>();

        TraverseDirectories(directoryPath, (currentDirPath) =>
        {
            var currentDirData = GetSingleDirectoryData(currentDirPath, false, false);
            mergedDirectoryData.AddRange(currentDirData.Images);
        });

        if (mergedDirectoryData.Any()) {
            result.ArchiveEntries.Add(directoryPath);
            result.ContactLists.Add(new ContactListData
            {
                Name = directoryPath,
                Images = mergedDirectoryData
            });
        }

        return result;
    }
}
